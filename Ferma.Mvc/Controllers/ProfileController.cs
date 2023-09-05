using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Data.Datacontext;
using Ferma.Mvc.ViewModels;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Mvc.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataContext _context;
        private readonly ISmsSenderServices _smsSenderServices;
        private readonly IProfileIndexServices _profileIndexServices;
        private readonly IBalanceIncreaseServices _balanceIncreaseServices;
        private readonly IPosterEditServices _posterEditServices;
        private readonly IProfileEditServices _profileEditServices;
        private readonly IPhoneNumberServices _numberServices;
        private readonly IProfileLoginServices _loginServices;
        private readonly IAuthenticationServices _authenticationServices;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public ProfileController(DataContext context, ISmsSenderServices smsSenderServices, IProfileIndexServices profileIndexServices, IBalanceIncreaseServices balanceIncreaseServices, IPosterEditServices posterEditServices, IProfileEditServices profileEditServices, IPhoneNumberServices numberServices, IProfileLoginServices loginServices, IAuthenticationServices authenticationServices, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _smsSenderServices = smsSenderServices;
            _profileIndexServices = profileIndexServices;
            _balanceIncreaseServices = balanceIncreaseServices;
            _posterEditServices = posterEditServices;
            _profileEditServices = profileEditServices;
            _numberServices = numberServices;
            _loginServices = loginServices;
            _authenticationServices = authenticationServices;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            if (user == null)
                return RedirectToAction("daxilol", "profile");

            var profileVM = await _profileIndexServices._profileVM(user);

            return View(profileVM);
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
                _numberServices.PhoneNumberPrefixValidation(number);

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


                    // Response yoxlanması
                    //if (await _smsSenderServices.SmsSend(number, code))
                    //{
                        //url
                        url = Url.Action("LoginAuthentication", "profile", new { phoneNumber = number, token = token }, Request.Scheme);
                        return Redirect(url);
                    //}
                }
            }
            catch (NotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("PhoneNumber", ex.Message);
                return View();
            }
            catch (SmsSenderException ex)
            {
                ModelState.AddModelError("PhoneNumber", ex.Message);
                return View();
            }
            catch (RareLimitException ex)
            {
                ModelState.AddModelError("PhoneNumber", ex.Message);
                return View();
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
            catch (NotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (ExpirationDateException ex)
            {
                ModelState.AddModelError("Code", ex.Message);
                return View(authenticationViewModel);
            }
            catch (AuthenticationCodeException ex)
            {
                ModelState.AddModelError("Code", ex.Message);
                return View(authenticationViewModel);
            }
            catch (Exception)
            {
                return View(authenticationViewModel);
            }

            return RedirectToAction("index", "anasehife");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Edit(int id)
        {
            var posterEditVM = await _profileEditServices.EditVM(id);

            return View(posterEditVM);
        }

        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(Poster poster)
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            var profileVM = await _profileIndexServices._profileVM(user);

            var editVM = await _profileEditServices.EditVM(poster.Id);

            try
            {
                _posterEditServices.posterEditCheck(poster);

                await _posterEditServices.posterEdit(poster);
            }

            catch (NotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException msg)
            {
                ModelState.AddModelError("", msg.Message);
                TempData["Error"] = (msg.Message);

                return View(editVM);

            }
            catch (ItemFormatException msg)
            {
                ModelState.AddModelError("", msg.Message);
                TempData["Error"] = (msg.Message);

                return View(editVM);

            }
            catch (ImageNullException msg)
            {
                ModelState.AddModelError("ImageFiles", msg.Message);
                TempData["Error"] = (msg.Message);

                return View(editVM);

            }
            catch (ImageFormatException msg)
            {
                ModelState.AddModelError("ImageFiles", msg.Message);
                TempData["Error"] = (msg.Message);
                return View(editVM);
            }

            catch (ImageCountException msg)
            {
                ModelState.AddModelError("ImageFiles", msg.Message);
                TempData["Error"] = (msg.Message);
                return View(editVM);
            }
            catch (ValueAlreadyExpception msg)
            {
                ModelState.AddModelError("", msg.Message);
                TempData["Error"] = (msg.Message);
                return View(editVM);

            }
            catch (Exception)
            {
                TempData["Error"] = ("Dəyişikliklər uğursuz oldu!");
                return RedirectToAction("index", profileVM);
            }

            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return RedirectToAction("index", "profile");
        }

        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditUser(ProfileEditDto ProfileEditDto)
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            var profileVM = await _profileIndexServices._profileVM(user);

            try
            {
                await _profileEditServices.CheckValue(ProfileEditDto);
                await _profileEditServices.Edit(ProfileEditDto);
            }

            catch (NotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException)
            {
                TempData["Error"] = ("Dəyişikliklər uğursuz oldu!");
                return RedirectToAction("index", profileVM);
            }
            catch (ValueAlreadyExpception)
            {
                TempData["Error"] = ("Dəyişikliklər uğursuz oldu!");
                return RedirectToAction("index", profileVM);

            }
            catch (Exception)
            {
                return RedirectToAction("index", profileVM);

            }

            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return RedirectToAction("index", "profile");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> DisabledPoster(int id)
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            var profileVM = await _profileIndexServices._profileVM(user);


            try
            {
                await _posterEditServices.posterDisabled(id);
            }

            catch (ItemNotFoundException msg)
            {
                TempData["Error"] = (msg.Message);
                return RedirectToAction("index", profileVM);
            }

            catch (Exception)
            {
                return RedirectToAction("index", profileVM);
            }

            TempData["Success"] = ("Elan deaktiv oldu");
            return RedirectToAction("index");

        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> ActivePoster(int id)
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            var profileVM = await _profileIndexServices._profileVM(user);


            try
            {
                await _posterEditServices.posterActive(id);
            }

            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (ExpirationDateException msg)
            {
                TempData["Error"] = (msg.Message);
                return RedirectToAction("index", profileVM);
            }
            catch (Exception)
            {
                return RedirectToAction("index", profileVM);
            }

            TempData["Success"] = ("Elan aktiv oldu");
            return RedirectToAction("index");

        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "anasehife");
        }


        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        private async Task<IActionResult> BalanceIncrease(BalanceDto balanceDto)
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            var profileVM = await _profileIndexServices._profileVM(user);
            try
            {
                await _balanceIncreaseServices.CheckBalanceIncrease(balanceDto);
                await _balanceIncreaseServices.BalanceIncrease(balanceDto);
            }

            catch (UserNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (PaymentValueException msg)
            {
                TempData["Error"] = (msg.Message);
                return RedirectToAction("index", profileVM);
            }

            catch (Exception)
            {
                return RedirectToAction("index", profileVM);
            }

            return RedirectToAction("index", "profile");
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
