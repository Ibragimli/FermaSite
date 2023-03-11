using Ferma.Core.Entites;
using Ferma.Data.Datacontext;
using Ferma.Mvc.ViewModels;
using Ferma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailServices _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ProfileController(DataContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailServices emailService, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _roleManager = roleManager;
        }
        public IActionResult DaxilOl()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> DaxilOl(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                ModelState.AddModelError("PhoneNumber", "Nömrə boş ola bilməz!");
                return View();
            }

            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            Random random = new Random();
            string code = random.Next(100000, 999999).ToString();
            string token = " ";
            string url = " ";

            //AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;


            string number = "";
            string[] charsNumber = phoneNumber.Split('_', '-', ' ', '(', ')', ',', '.', '/', '?', '!', '+', '=', '|', '.');

            foreach (var item in charsNumber)
            {
                number += item;
            }
            var UserExists = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == number);
            //Hesab olmuyanda
            if (UserExists == null)
            {
                AppUser newUser = new AppUser
                {
                    UserName = number,
                    PhoneNumber = number,
                    IsAdmin = false,
                    Balance = 0,
                };
                var result = await _userManager.CreateAsync(newUser, code);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("PhoneNumber", error.Description);
                    }
                    return View();
                }
                await _userManager.AddToRoleAsync(newUser, "User");
                //_emailService.Send("elnur204@list.ru", "Təsdiqləmə kodunuz", code); SMS GONDER
                token = new string(Enumerable.Repeat(chars, 30).Select(s => s[random.Next(s.Length)]).ToArray());
                _autenticaitonCreate(token, code, number);
                //url
                url = Url.Action("LoginAuthentication", "profile", new { phoneNumber = number, token = token }, Request.Scheme);
                return Redirect(url);
            }

            //Hesab olanda
            if (!UserExists.IsAdmin)
            {
                var tokenResetPassword = await _userManager.GeneratePasswordResetTokenAsync(UserExists);

                if (UserExists == null || !(await _userManager.VerifyUserTokenAsync(UserExists, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", tokenResetPassword)))
                {
                    return RedirectToAction("notFound");
                }

                var result = await _userManager.ResetPasswordAsync(UserExists, tokenResetPassword, code);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }

                token = new string(Enumerable.Repeat(chars, 30).Select(s => s[random.Next(s.Length)]).ToArray());
                _autenticaitonCreate(token, code, number);

                //_emailService.Send("elnur204@list.ru", "Təsdiqləmə kodunuz", code); SMS GONDER

                //url
                url = Url.Action("LoginAuthentication", "profile", new { phoneNumber = number, token = token }, Request.Scheme);

                return Redirect(url);
            }

            return View();
        }



        public IActionResult LoginAuthentication(string phoneNumber, string token)
        {

            var authentication = _context.UserAuthentications.Where(x => x.IsDisabled == false).FirstOrDefault(x => x.Token == token && x.PhoneNumber == phoneNumber);

            if (authentication == null)
                return RedirectToAction("NotFound");

            var authenticationViewModel = _autenticaitonVM(authentication.Token, authentication.PhoneNumber);

            return View(authenticationViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> LoginAuthentication(string code, string phoneNumber, string token)
        {
            var authentication = _context.UserAuthentications.Where(x => x.IsDisabled == false).FirstOrDefault(x => x.Code == code && x.Token == token);
            var authenticationViewModel = _autenticaitonVM(token, phoneNumber);
            var existAuthentication = await _context.UserAuthentications.Where(x => x.IsDisabled == false).FirstOrDefaultAsync(x => x.Token == token);
            if (authentication == null)
            {
                if (existAuthentication != null)
                {
                    if (existAuthentication.Count > 1)
                        existAuthentication.Count -= 1;

                    else
                    {
                        existAuthentication.IsDisabled = true;

                    }
                    _context.SaveChanges();
                }
                ModelState.AddModelError("code", "Kod yanlışdır!");

                return View(authenticationViewModel);
            }

            var UserExists = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == phoneNumber && x.IsAdmin == false);
            if (UserExists == null)
                return RedirectToAction("notfound");

            var result = await _signInManager.PasswordSignInAsync(UserExists, code, false, false);
            if (!result.Succeeded)
                return RedirectToAction("notfound");

            authentication.IsDisabled = true;
            _context.SaveChanges();
            return RedirectToAction("index", "anasehife");
        }


        [Authorize(Roles = "User")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "anasehife");
        }
        private LoginAuthenticationViewModel _autenticaitonVM(string token, string phoneNumber)
        {
            LoginAuthenticationViewModel authenticationViewModel = new LoginAuthenticationViewModel
            {
                Token = token,
                PhoneNumber = phoneNumber,
            };
            return authenticationViewModel;
        }

        private UserAuthentication _autenticaitonCreate(string token, string code, string phoneNumber)
        {
            var oldAuthentication = _context.UserAuthentications.Where(x => x.IsDisabled == false && x.PhoneNumber == phoneNumber).ToList();
            if (oldAuthentication != null)
            {
                foreach (var item in oldAuthentication)
                {
                    item.IsDisabled = true;
                }
            }

            UserAuthentication authentication = new UserAuthentication
            {
                Code = code,
                Token = token,
                IsDisabled = false,
                PhoneNumber = phoneNumber,
                Count = 3,
            };
            _context.Add(authentication);
            _context.SaveChanges();
            return authentication;
        }
    }
}
