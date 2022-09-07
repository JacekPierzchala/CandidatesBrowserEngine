using CandidatesBrowserEngine.Models;
using CandidatesBrowserEngine.Models.ViewModels.Account;
using CandidatesBrowserEngine.Services;
using CandidatesBrowserEngine.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Controllers
{
    [Authorize(Roles =Helper.Admin)]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(UserManager<ApplicationUser> userManager, IUserService userService, IRoleService roleService)
        {
            _userManager = userManager;
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userService.GetUserById(userId);
            if (string.IsNullOrEmpty(user.Id))
            {
                return NotFound();
            }

            var roles = await _roleService.GetAllRoles();
 
            var userModel = new EditUserViewModel
            {
                 UserViewModel = user,
                 RoleList= roles.Select(u=> new SelectListItem 
                 {
                     Text = u.Name,
                     Value = u.Id

                 })
            };
            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
          

                var result= await _userService.UpdateUser(viewModel.UserViewModel);
                if(result)
                {
                    TempData[Helper.Success] = "User updated successfully";
                }
                else
                {
                    TempData[Helper.Error] = "User update error";
                }
                return RedirectToAction(nameof(Index));
            }

            var roles = await _roleService.GetAllRoles();
            viewModel.RoleList = roles.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id

            });

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUnlock(string userId)
        {
            var result = await _userService.ChangeLockUser(userId);
            if (result)
            {
                TempData[Helper.Success] = "User lock info changed successfully";
            }
            else
            {
                TempData[Helper.Error] = "User lock info change error";
            }
            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string userId)
        {
            var result = await _userService.DeleteUser(userId);
            if (result)
            {
                TempData[Helper.Success] = "User deleted successfull";
            }
            else
            {
                TempData[Helper.Error] = "User removal error";
            }
            return RedirectToAction(nameof(Index));

        }
    }
}
