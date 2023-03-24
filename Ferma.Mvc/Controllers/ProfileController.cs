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
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataContext _context;
        private readonly IPosterEditServices _posterEditServices;
        private readonly IProfileEditServices _profileEditServices;
        private readonly IPhoneNumberServices _numberServices;
        private readonly IProfileLoginServices _loginServices;
        private readonly IAuthenticationServices _authenticationServices;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public ProfileController(DataContext context, IPosterEditServices posterEditServices, IProfileEditServices profileEditServices, IPhoneNumberServices numberServices, IProfileLoginServices loginServices, IAuthenticationServices authenticationServices, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
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

            var profileVM = await _profileVM(user);

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
        public async Task<IActionResult> Edit(int id)
        {
            PosterEditViewModel posterEditVM = new PosterEditViewModel
            {
                PosterEditDto = new PosterEditDto(),
                Poster = await _context.Posters.Include(x => x.PosterFeatures)
                .Include(x => x.PosterUserIds).ThenInclude(x => x.AppUser)
                .Include(x => x.PosterFeatures).ThenInclude(x => x.City)
                .Include(x => x.PosterFeatures).ThenInclude(x => x.SubCategory)
                .Include(x => x.PosterFeatures).ThenInclude(x => x.SubCategory.Category)
                .Include(x => x.PosterImages).Where(x => x.IsDelete == false && x.PosterFeatures.IsDisabled == false)
                .FirstOrDefaultAsync(x => x.Id == id),
                Categories = _context.Categories.ToList(),
                SubCategories = _context.SubCategories.ToList(),
                Cities = _context.Cities.ToList(),
            };

            return View(posterEditVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(Poster poster)
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            var profileVM = await _profileVM(user);

            try
            {
                await _posterEditServices.posterEdit(poster);
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


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditUser(ProfileEditDto ProfileEditDto)
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            var profileVM = await _profileVM(user);

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


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> disabledPoster(int id)
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            var profileVM = await _profileVM(user);

            try
            {
                await _posterEditServices.posterDisabled(id);
            }

            catch (ItemNotFoundException msg)
            {
                TempData["Success"] = (msg.Message);
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

        private async Task<ProfileViewModel> _profileVM(AppUser user)
        {
            ProfileViewModel profileVM = new ProfileViewModel
            {
                User = user,
                ActivePosters = await _context.Posters.Include(x => x.PosterImages).Include(x => x.PosterFeatures).ThenInclude(x => x.City).Where(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Active).Take(50).ToListAsync(),
                DeactivePosters = await _context.Posters.Include(x => x.PosterImages).Include(x => x.PosterFeatures).ThenInclude(x => x.City).Where(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.DeActive).Take(50).ToListAsync(),
                WaitedPosters = await _context.Posters.Include(x => x.PosterImages).Include(x => x.PosterFeatures).ThenInclude(x => x.City).Where(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Waiting).Take(50).ToListAsync(),
                DisabledPosters = await _context.Posters.Include(x => x.PosterImages).Include(x => x.PosterFeatures).ThenInclude(x => x.City).Where(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Disabled).Take(50).ToListAsync(),
                PersonalPayments = await _context.Payments.Include(x => x.AppUser).Where(x => !x.IsDelete && x.Service == PaymentService.BalancePayment && x.AppUserId == user.Id).ToListAsync(),
                PosterPayments = await _context.Payments.Include(x => x.AppUser).Include(x => x.Posters).ThenInclude(x => x.PosterFeatures).Where(x => !x.IsDelete && x.Service == PaymentService.PosterPayment && x.AppUserId == user.Id).ToListAsync(),
                ProfileEditDto = new ProfileEditDto(),
            };
            return profileVM;
        }

    }
}
