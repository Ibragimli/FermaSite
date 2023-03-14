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
        private readonly IPosterCreateValueCheckServices _posterCreateValueCheckServices;
        private readonly IAuthenticationServices _autenticationServices;
        private readonly IManageImageHelper _imageHelper;
        private readonly IPosterCreateIndexServices _posterIndexServices;
        private readonly IPosterCreateServices _createServices;
        private readonly DataContext _context;

        public ElanlarController(UserManager<AppUser> userManager, IPosterCreateValueCheckServices posterCreateValueCheckServices, IAuthenticationServices autenticationServices, IManageImageHelper imageHelper, IPosterCreateIndexServices posterIndexServices, IPosterCreateServices createServices, DataContext context)
        {
            _userManager = userManager;
            _posterCreateValueCheckServices = posterCreateValueCheckServices;
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
                //----------------------------------------
                // Inputlari yoxlama hissesi

                //SubCategory check 
                _posterCreateValueCheckServices.SubCategoryValidation(posterCreateDto.SubCategoryId);
                _posterCreateValueCheckServices.CityValidation(posterCreateDto.CityId);

                //nomre yoxlanilmasi
                _posterCreateValueCheckServices.PhoneNumberValidation(posterCreateDto.PhoneNumber);

                //sekil yoxlanilmasi
                _posterCreateValueCheckServices.ImageCheck(posterCreateDto.ImageFiles);


                //nomre filterlemesi
                posterCreateDto.PhoneNumber = _createServices.PhoneNumberFilter(posterCreateDto.PhoneNumber);


                //CheckImage
                _imageHelper.ImagesCheck(posterCreateDto.ImageFiles);

                // Inputlari yoxlama hissesi
                //----------------------------------------

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
                    var authentication = await _createServices.CreateAuthentication(token, code, posterCreateDto.PhoneNumber);

                    //Link
                    url = Url.Action("NumberAuthentication", "elanlar", new { phoneNumber = posterCreateDto.PhoneNumber, token = token }, Request.Scheme);

                    //Cookie yaradilmasi
                    _createServices.CreatePosterCookie(posterCreateDto.ImageFiles, posterCreateDto);
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
                    _imageHelper.ImagesCheck(posterCreateDto.ImageFiles);


                    feature = await _createServices.CreatePosterFeature(posterCreateDto);
                    poster = await _createServices.CreatePosterForm(feature, posterCreateDto.ImageFiles);

                    //Create
                    await _createServices.CreateImageFormFile(posterCreateDto.ImageFiles, poster.Id);

                    await _createServices.CreatePosterUserId(userExist.Id, poster.Id, userExist);

                }


            }
            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(posterCreateView);

            }
            catch (ItemNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(posterCreateView);

            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(posterCreateView);

            }
            catch (ImageFormatException ex)
            {
                ModelState.AddModelError("PosterCreateDto.ImageFiles", ex.Message);
                return View(posterCreateView);
            }
            catch (ImageNullException ex)
            {
                ModelState.AddModelError("PosterCreateDto.ImageFiles", ex.Message);
                return View(posterCreateView);
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
                await _createServices.CreateImageString(images, poster.Id);
                await _createServices.CreatePosterUserId(user.Id, poster.Id, user);

                await _createServices.ChangeAuthenticationStatus(authentication);
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

    }
}
