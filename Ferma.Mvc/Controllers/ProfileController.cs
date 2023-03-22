using Ferma.Core.Entites;
using Ferma.Data.Datacontext;
using Ferma.Mvc.ViewModels;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Services.Interfaces;
using Ferma.Service.Services.Interfaces.User;
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
        private readonly IPhoneNumberServices _numberServices;
        private readonly IProfileLoginServices _loginServices;
        private readonly IAuthenticationServices _authenticationServices;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public ProfileController(DataContext context, IPhoneNumberServices numberServices, IProfileLoginServices loginServices, IAuthenticationServices authenticationServices, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _numberServices = numberServices;
            _loginServices = loginServices;
            _authenticationServices = authenticationServices;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> DaxilOl()
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            if (user != null && user.IsAdmin == false)
            {
                return RedirectToAction("index", "anasehife");
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> DaxilOl(string phoneNumber)
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            if (user != null && user.IsAdmin == false)
            {
                return RedirectToAction("index", "anasehife");
            }

            if (phoneNumber == null)
            {
                ModelState.AddModelError("PhoneNumber", "Nömrə boş ola bilməz!");
                return View();
            }

            string token = " ";
            string url = " ";
            try
            {
                _numberServices.PhoneNumberValidation(phoneNumber);

                //phone filter
                string number = _numberServices.PhoneNumberFilter(phoneNumber);
                //random code
                string code = _authenticationServices.CodeCreate();
                var UserExists = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == number);
                //Hesab olmuyanda
                if (UserExists == null)
                {
                    //user yaratmaq
                    await _loginServices.UserCreate(number, code);

                    //token yaradilmasi
                    token = _authenticationServices.CreateToken();

                    //tesdiqleme modelinin yaranmasi
                    var authentication = await _authenticationServices.CreateAuthentication(token, code, number);

                    //url
                    url = Url.Action("LoginAuthentication", "profile", new { phoneNumber = number, token = token }, Request.Scheme);
                    return Redirect(url);
                }

                //Hesab olanda
                if (!UserExists.IsAdmin)
                {

                    //token yaradilmasi
                    token = _authenticationServices.CreateToken();

                    //tesdiqleme modelinin yaranmasi
                    var authentication = await _authenticationServices.CreateAuthentication(token, code, number);
                    await _loginServices.UserResetPassword(number, code);

                    //url
                    url = Url.Action("LoginAuthentication", "profile", new { phoneNumber = number, token = token }, Request.Scheme);

                    return Redirect(url);
                }
            }
            catch (NotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
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
            var authenticationViewModel = _autenticaitonVM(token, phoneNumber);

            try
            {
                var authentication = await _loginServices.LoginAuthentication(code, phoneNumber, token);
                await _loginServices.UserLogin(phoneNumber, code, authentication);
            }
            catch (AuthenticationCodeException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(authenticationViewModel);
            }
            catch (NotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (Exception)
            {
                return View(authenticationViewModel);
            }

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

    }
}
