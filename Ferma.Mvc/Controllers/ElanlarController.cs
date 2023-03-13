using Ferma.Core.Entites;
using Ferma.Data.Datacontext;
using Ferma.Mvc.ViewModels;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.Helper;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Ferma.Mvc.Controllers
{
    public class ElanlarController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthenticationServices _autenticationServices;
        private readonly IManageImageHelper _imageHelper;
        private readonly IPosterCreateIndexServices _posterIndexServices;
        private readonly IPosterCreateServices _createServices;
        private readonly DataContext _context;

        public ElanlarController(UserManager<AppUser> userManager, IAuthenticationServices autenticationServices, IManageImageHelper imageHelper, IPosterCreateIndexServices posterIndexServices, IPosterCreateServices createServices, DataContext context)
        {
            _userManager = userManager;
            _autenticationServices = autenticationServices;
            _imageHelper = imageHelper;
            _posterIndexServices = posterIndexServices;
            _createServices = createServices;
            _context = context;
        }
        public IActionResult YeniElan()
        {
            var posterCreateView = _posterVM();


            //PosterCreateViewModels posterCreateView = new PosterCreateViewModels
            //{
            //    PosterCreateDto = new PosterCreateDto(),
            //    Categories = await _posterIndexServices.Categories(),
            //    SubCategories = await _posterIndexServices.SubCategories(),
            //    Cities = await _posterIndexServices.Cities(),
            //};
            return View(posterCreateView);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> YeniElan(PosterCreateDto posterCreateDto)
        {
            var posterCreateView = _posterVM();

            PosterFeatures feature;
            Poster poster;
            string url;
            posterCreateDto.ImageFilesStr = new List<string>();

            try
            {

                // inputlari yoxlama hissesi

                //nomre filterlemesi
                posterCreateDto.PhoneNumber = _createServices.PhoneNumberFilter(posterCreateDto.PhoneNumber);

                ////CheckImage
                _imageHelper.ImagesCheck(posterCreateDto.PosterImageFiles.ImageFiles);

                // inputlari yoxlama hissesi

                //6-reqemli kodun yaradılması
                var code = _autenticationServices.CodeCreate();

                //User-in  daxil olub olmamasinin yoxlanilmasi
                AppUser userExist = User.Identity.IsAuthenticated ? _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin) : null;

                //Hesaba daxil olunmayibsa
                if (userExist == null)
                {
                    //tokenin yaradilması
                    var token = _autenticationServices.CreateToken();

                    //tesdiqleme modelinin yaranmasi
                    var authentication = _autenticaitonCreate(token, code, posterCreateDto.PhoneNumber);

                    //Link
                    url = Url.Action("NumberAuthentication", "elanlar", new { phoneNumber = posterCreateDto.PhoneNumber, token = token }, Request.Scheme);

                    //Cookie yaradilmasi
                    _createServices.CreatePosterCookie(posterCreateDto.PosterImageFiles.ImageFiles, posterCreateDto);
                    return Redirect(url);

                    //foreach (var item in posterCreateDto.PosterImageFiles.ImageFiles)
                    //{
                    //    var filename = _imageHelper.FileSave(item, "poster");
                    //    posterCreateDto.ImageFilesStr.Add(filename);
                    //}

                    //var posterImageStr = JsonConvert.SerializeObject(posterCreateDto.ImageFilesStr);
                    //HttpContext.Response.Cookies.Append("posterImageFiles", posterImageStr);
                    //posterCreateDto.PosterImageFiles = null;
                    //var posterStr = JsonConvert.SerializeObject(posterCreateDto);
                    //HttpContext.Response.Cookies.Append("posterVM", posterStr);
                }
                //Hesaba daxil olubsa
                else
                {
                    userExist.Name = posterCreateDto.UserName;
                    if (posterCreateDto.Email != null)
                        userExist.Email = posterCreateDto.Email;
                    //Check
                    _imageHelper.ImagesCheck(posterCreateDto.PosterImageFiles.ImageFiles);


                    feature = await _createServices.CreatePosterFeature(posterCreateDto);
                    poster = await _createServices.CreatePosterForm(feature, posterCreateDto.PosterImageFiles.ImageFiles);

                    //Create
                     //_createServices.CreateImageFormFile(posterCreateDto.PosterImageFiles.ImageFiles, poster.Id);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                //TempData["Error"] = ("Proses uğursuz oldu!");
                return View(posterCreateView);
            }
            return RedirectToAction("index", "anasehife");

        }
        public IActionResult NumberAuthentication(string phoneNumber, string token)
        {
            var authentication = _context.UserAuthentications.Where(x => x.IsDisabled == false).FirstOrDefault(x => x.Token == token && x.PhoneNumber == phoneNumber);

            if (authentication == null)
                return RedirectToAction("index", "notfound");

            var authenticationViewModel = _autenticaitonVM(authentication.Token, authentication.PhoneNumber);

            return View(authenticationViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> NumberAuthentication(string code, string phoneNumber, string token)
        {
            PosterCreateDto posterCreateDto = new PosterCreateDto();
            List<string> images = new List<string>();
            var authenticationViewModel = _autenticaitonVM(token, phoneNumber);
            AppUser user = new AppUser();
            PosterUserId posterUserId = new PosterUserId();
            UserAuthentication authentication = new UserAuthentication();

            try
            {
                authentication = await _createServices.CheckAuthentication(code, phoneNumber, token);

                //postercookie
                posterCreateDto = _createServices.GetPosterCookie();

                //imagescookie
                images = _createServices.GetImageFilesCookie();

                user = await _createServices.CreateNewUser(code, phoneNumber, posterCreateDto.Email, posterCreateDto.UserName);


                var feature = await _createServices.CreatePosterFeature(posterCreateDto);

                var poster = await _createServices.CreatePoster(feature);

                //Şəkil yaratmaq prosesi
                _createServices.CreateImageString(images, poster.Id);
                _createServices.CreatePosterUserId(user.Id, poster.Id, user);

                _createServices.ChangeAuthenticationStatus(authentication);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("code", ex.Message);
                //TempData["Error"] = ("Proses uğursuz oldu!");
                return View(authenticationViewModel);
            }

            TempData["Success"] = "Elanınız yaradıldı, zəhmət olmasa elanınızın təsdiqlənməsini gözləyin";
            return RedirectToAction("index", "anasehife");
        }


        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public async Task<IActionResult> NumberAuthentication(string code, string phoneNumber, string token)
        //{
        //    var authentication = _context.UserAuthentications.Where(x => x.IsDisabled == false).FirstOrDefault(x => x.Code == code && x.Token == token && x.PhoneNumber == phoneNumber);
        //    var existAuthentication = _context.UserAuthentications.Where(x => x.IsDisabled == false).FirstOrDefault(x => x.Token == token);
        //    var authenticationViewModel = _autenticaitonVM(token, phoneNumber);

        //    //Kod yanlişdir erroru və təkrar yoxlama limiti
        //    if (authentication == null)
        //    {
        //        if (existAuthentication != null)
        //        {
        //            if (existAuthentication.Count > 1)
        //                existAuthentication.Count -= 1;
        //            else
        //            {
        //                existAuthentication.IsDisabled = true;
        //            }
        //            _context.SaveChanges();
        //        }
        //        ModelState.AddModelError("code", "Kod yanlışdır!");
        //        return View(authenticationViewModel);
        //    }

        //    PosterCreateDto posterCreateDto = new PosterCreateDto();
        //    List<string> images = new List<string>();

        //    //cookie
        //    string posterItem = HttpContext.Request.Cookies["posterVM"];
        //    string imageItem = HttpContext.Request.Cookies["posterImageFiles"];

        //    if (posterItem != null)
        //    {
        //        posterCreateDto = JsonConvert.DeserializeObject<PosterCreateDto>(posterItem);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("code", "Cookie-nizi aktiv edin!");
        //        return View(authenticationViewModel);
        //    }

        //    if (imageItem != null)
        //    {
        //        images = JsonConvert.DeserializeObject<List<string>>(imageItem);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("code", "Cookie-nizi aktiv edin!");
        //        return View(authenticationViewModel);
        //    }

        //    AppUser newUser = new AppUser();
        //    //AppUser UserExists = new AppUser();
        //    //hesab yaradmaq

        //    var UserExists = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        //    if (UserExists == null)
        //    {
        //        newUser = new AppUser
        //        {
        //            UserName = phoneNumber,
        //            PhoneNumber = phoneNumber,
        //            IsAdmin = false,
        //            Balance = 0,
        //            Email = posterCreateDto.Email,
        //            Name = posterCreateDto.UserName,
        //        };
        //        var result = await _userManager.CreateAsync(newUser, code);
        //        if (!result.Succeeded)
        //        {
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("code", error.Description);
        //            }
        //            return View(authenticationViewModel);
        //        }
        //        await _userManager.AddToRoleAsync(newUser, "User");
        //        _context.SaveChanges();
        //    }
        //    //hesab yaradmaq


        //    var feature = await _createServices.CreatePosterFeature(posterCreateDto);

        //    var poster = await _createServices.CreatePoster(feature);

        //    //Şəkil yaratmaq prosesi
        //    _createServices.CreateImageString(images, poster.Id);


        //    PosterUserId posterUserId = new PosterUserId();

        //    //poster user elaqesi
        //    if (UserExists == null)
        //    {
        //        posterUserId = new PosterUserId
        //        {
        //            AppUserId = newUser.Id,
        //            PosterId = poster.Id,
        //        };
        //    }
        //    //poster user elaqesi
        //    else
        //    {
        //        posterUserId = new PosterUserId
        //        {
        //            AppUserId = UserExists.Id,
        //            PosterId = poster.Id,
        //        };
        //    }
        //    _context.PosterUserIds.Add(posterUserId);

        //    //kodun deaktiv olunmasi
        //    authentication.IsDisabled = true;

        //    _context.SaveChanges();
        //    TempData["Success"] = "Elanınız yaradıldı, zəhmət olmasa elanınızın təsdiqlənməsini gözləyin";
        //    return RedirectToAction("index", "anasehife");
        //}

        private PosterCreateViewModels _posterVM()
        {
            PosterCreateViewModels posterCreateView = new PosterCreateViewModels
            {
                PosterCreateDto = new PosterCreateDto(),
                Categories = _context.Categories.ToList(),
                SubCategories = _context.SubCategories.ToList(),
                Cities = _context.Cities.ToList(),
            };
            return posterCreateView;
        }
        private NumberAuthenticationViewModel _autenticaitonVM(string token, string phoneNumber)
        {
            NumberAuthenticationViewModel authenticationViewModel = new NumberAuthenticationViewModel
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
