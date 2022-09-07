using CandidatesBrowserEngine.Models;
using CandidatesBrowserEngine.Models.ViewModels;
using CandidatesBrowserEngine.Models.ViewModels.Account;
using CandidatesBrowserEngine.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly UrlEncoder _urlEncoder;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, UrlEncoder urlEncoder, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _urlEncoder = urlEncoder;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,lockoutOnFailure:true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    user.LastAccess = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    HttpContext.Session.SetString("ssuserName",user.Name);
                    return RedirectToAction("Index", "CandidatesMainView");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(VerifyAuthenticatorCode), new { returnUrl = returnUrl, rememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    return View("LockOut");
                }
                else
                {
                    _logger.LogError("Invalid login credentials provided");
                    ModelState.AddModelError("", "Invalid login credentials provided");
                }
  
            }
            return View(model);
        }

        public IActionResult Register(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl=null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    DateCreated=DateTime.Now
                };
                try
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");
                        await _userManager.AddToRoleAsync(user, Helper.User);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                        await _emailSender.SendEmailAsync(model.Email, "Confirm you account Candidates Browser Engine",
                            "Please confirm you account in Candidates Browser Engine by clicking here:  <a href=\"" + callbackUrl + "\">link</a>");
                        TempData[Helper.Success] = "Please check your mailbox and complete registration process";
                        return RedirectToAction(nameof(Login),"Account");            
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
                catch (Exception ex)
                {
                    await _userManager.DeleteAsync(user);
                    _logger.LogError(ex.Message);
                    return View(model);
                }             
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logoff()
        {
            await _signInManager.SignOutAsync();
          
            return RedirectToAction(nameof(Login), "Account");
        }


        [HttpGet]
        public async Task<IActionResult> LogoffTimeout()
        {
            await _signInManager.SignOutAsync();
            TempData[Helper.Error] = "Current session has ended due to inacivity";

            return RedirectToAction(nameof(Login), "Account");
        }


        [HttpGet]
        public IActionResult  ResendEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendEmail(ResendEmailConfirmationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);
                if (user == null)
                {
                    _logger.LogError("Email confirmation failed. User does not exist");
                    TempData[Helper.Success] = "Please check your mailbox and complete registration process";
                    return RedirectToAction(nameof(Login), "Account");
                }
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(viewModel.Email, "Confirm you account Candidates Browser Engine",
                 "Please confirm you account in Candidates Browser Engine by clicking here:  <a href=\"" + callbackUrl + "\">link</a>");

                _logger.LogError("Email confirmation sent");
                TempData[Helper.Success] = "Please check your mailbox and complete registration process";
                return RedirectToAction(nameof(Login), "Account");
            }

            TempData[Helper.Success] = "Please check your mailbox and complete registration process";
            return RedirectToAction(nameof(Login), "Account");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                _logger.LogError("email confirmation failed");
                return RedirectToAction(nameof(Login), "Account");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("email confirmation failed");  
                return RedirectToAction(nameof(Login), "Account");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if(result.Succeeded)
            {
                _logger.LogInformation("email confirmation success");
            }
            else
            {
                _logger.LogError("email confirmation failed");
            }
            return RedirectToAction(nameof(Login), "Account");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);
                if (user == null)
                {
                    RedirectToAction("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(viewModel.Email, "Reset password Candidates Browser Engine",
                    "Please reset you password by clicking here:  <a href=\"" + callbackUrl + "\">link</a>");

                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);
                if (user == null)
                {
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }

                var result = await _userManager.ResetPasswordAsync(user, viewModel.Code, viewModel.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User reset password");
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }
                _logger.LogInformation("User failed to password");
                AddErrors(result);
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLoginSignUp(string provider, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var redirect = Url.Action(nameof(ExternalLoginCallbackSignUp), "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirect);
            ViewData["ReturnUrl"] = returnUrl;
            return Challenge(properties, provider);

        }


        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallbackSignUp(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Register));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                return LocalRedirect(returnUrl);
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("VerifyAuthenticatorCode", new { returnUrl = returnUrl });
            }

            else
            {



                ViewData["ReturnUrl"] = returnUrl;
                ViewData["ProviderDisplayName"] = info.ProviderDisplayName;
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                string picture = null;
                switch (info.ProviderDisplayName)
                {
                    case "Facebook":
                        {
                            var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                            picture = $"https://graph.facebook.com/{identifier}/picture?type=large";

                        }
                        break;
                    case "Google":
                        {
                            if (info.Principal.HasClaim(c => c.Type == "image"))
                            {
                                picture = info.Principal.FindFirst("image").Value;
                            }

                        }
                        break;

                }

                return View(nameof(ExternalLoginConfirmationSignUp), new ExternalLoginConfirmationViewModel { Email = email, Name = name, Picture=picture });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmationSignUp(ExternalLoginConfirmationViewModel viewModel, string returnUrl = null, string provider=null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("Error");
                }
                var user = new ApplicationUser { Email = viewModel.Email, UserName = viewModel.Email, Name = viewModel.Name,  DateCreated = DateTime.Now, LastAccess=DateTime.Now };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, Helper.User);
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                       
                        await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                        return LocalRedirect(returnUrl);
                    }
                }
                AddErrors(result);
            }
            ViewData["ProviderDisplayName"] = provider;
            ViewData["ReturnUrl"] = returnUrl;
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLoginSignIn(string provider, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var redirect = Url.Action(nameof(ExternalLoginCallbackSignIn), "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirect);
            ViewData["ReturnUrl"] = returnUrl;
            return Challenge(properties, provider);

        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallbackSignIn(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
           
            if (result.Succeeded)
            {
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                return LocalRedirect(returnUrl);
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("VerifyAuthenticatorCode", new { returnUrl = returnUrl });
            }

            else
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["ProviderDisplayName"] = info.ProviderDisplayName;
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                string picture=null;
                switch (info.ProviderDisplayName)
                { 
                    case "Facebook":
                    {
                       var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                       picture = $"https://graph.facebook.com/{identifier}/picture?type=large";

                    }
                    break;
                    case "Google":
                    {
                      if (info.Principal.HasClaim(c => c.Type == "image"))
                      {
                          picture = info.Principal.FindFirst("image").Value;
                      }

                    }
                    break;

                }

               
                return View(nameof(ExternalLoginConfirmationSignIn), new ExternalLoginConfirmationViewModel { Email = email, Name = name,Picture=picture });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmationSignIn(ExternalLoginConfirmationViewModel viewModel, string returnUrl = null, string providerName=null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("Error");
                }
                var user = await _userManager.FindByNameAsync(viewModel.Email);
                if(user!=null)
                {
                    if(user.LockoutEnd!=null && user.LockoutEnd>DateTimeOffset.Now)
                    {
                        return View("LockOut");
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    user.LastAccess = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                   
                    _logger.LogInformation("user login success");
                    HttpContext.Session.SetString("ssuserName", user.Name);
                    
                    return RedirectToAction("Index", "CandidatesMainView");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                    _logger.LogError("user login failed");

                }

          
            }
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["ProviderDisplayName"] = providerName;
            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            string AuthenticatorUrlFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

            var user = await _userManager.GetUserAsync(User);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            var token = await _userManager.GetAuthenticatorKeyAsync(user);
            string AuthenticatorUrl = string.Format(AuthenticatorUrlFormat, _urlEncoder.Encode("CandidatesBrowserEngine"),
                _urlEncoder.Encode(user.Email), token);
            var model = new TwoFactorAuthenticationViewModel { Token = token, QRCodeUrl = AuthenticatorUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(TwoFactorAuthenticationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var success = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, model.Code);
                if (success)
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, true);
                }
                else
                {
                    ModelState.AddModelError("Verify", "You two factor authentication could not be enabled");
                    return View(model);
                }
            }
            return RedirectToAction(nameof(AuthenticatorConfirmation));
        }


        [HttpGet]
        public IActionResult AuthenticatorConfirmation()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> VerifyAuthenticatorCode(bool rememberMe, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(new VerifyAuthenticatorViewModel { RememberMe = rememberMe, ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyAuthenticatorCode(VerifyAuthenticatorViewModel model)
        {
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(model.Code, model.RememberMe, rememberClient: false);
            if (result.Succeeded)
            {
                return LocalRedirect(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code");
                return View(model);
            }
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
