using CandidatesBrowserEngine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Controllers
{
    [Authorize]
    public class AccountSettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountSettingsController(UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task <IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ViewData["TwofactorEnabled"] = false;
            }
            else
            {
                ViewData["TwofactorEnabled"] = user.TwoFactorEnabled;
            }
            return View();
        }
    }
}
