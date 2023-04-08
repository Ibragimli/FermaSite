using Ferma.Core.Entites;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.Area;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAdminLoginServices _adminLoginServices;

        public AccountController(UserManager<AppUser> userManager, IAdminLoginServices adminLoginServices)
        {
            _userManager = userManager;
            _adminLoginServices = adminLoginServices;
        }
        public async Task<IActionResult> Login()
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            if (user != null && user.IsAdmin == true)
            {
                return RedirectToAction("index", "dashboard");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginPostDto adminLoginPostDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _adminLoginServices.Login(adminLoginPostDto);
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

            return RedirectToAction("index", "dashboard");
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Logout()
        {
            _adminLoginServices.Logout();
            return RedirectToAction("login", "account");
        }


        //private async Task<IActionResult> CreateRole()
        //{
            //var role1 = await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            //var role2 = await _roleManager.CreateAsync(new IdentityRole("Admin"));
            //var role3 = await _roleManager.CreateAsync(new IdentityRole("Member"));

            //AppUser SuperAdmin = new AppUser { Name = "Elnur", UserName = "SuperAdmin" };
            //var admin = await _userManager.CreateAsync(SuperAdmin, "admin123");
            //var resultRole = await _userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");
            //return Ok(resultRole);
        //}

    }
}
