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
        private readonly IPosterSearchServices _searchServices;
        private readonly IPosterWishlistAddServices _posterWishlistAddServices;
        private readonly IPosterWishlistDeleteServices _posterWishlistDeleteServices;
        private readonly IPhoneNumberServices _numberServices;
        private readonly IPaymentCreateServices _paymentCreateServices;
        private readonly IElanDetailIndexServices _elanDetailIndexServices;
        private readonly IPosterCreateValueCheckServices _posterCreateValueCheckServices;
        private readonly IAuthenticationServices _autenticationServices;
        private readonly IManageImageHelper _imageHelper;
        private readonly IPosterCreateIndexServices _posterIndexServices;
        private readonly IPosterCreateServices _createServices;
        private readonly DataContext _context;

        public ElanlarController(UserManager<AppUser> userManager, IPosterSearchServices searchServices, IPosterWishlistAddServices posterWishlistAddServices, IPosterWishlistDeleteServices posterWishlistDeleteServices, IPhoneNumberServices numberServices, IPaymentCreateServices paymentCreateServices, IElanDetailIndexServices elanDetailIndexServices, IPosterCreateValueCheckServices posterCreateValueCheckServices, IAuthenticationServices autenticationServices, IManageImageHelper imageHelper, IPosterCreateIndexServices posterIndexServices, IPosterCreateServices createServices, DataContext context)
        {
            _userManager = userManager;
            _searchServices = searchServices;
            _posterWishlistAddServices = posterWishlistAddServices;
            _posterWishlistDeleteServices = posterWishlistDeleteServices;
            _numberServices = numberServices;
            _paymentCreateServices = paymentCreateServices;
            _elanDetailIndexServices = elanDetailIndexServices;
            _posterCreateValueCheckServices = posterCreateValueCheckServices;
            _autenticationServices = autenticationServices;
            _imageHelper = imageHelper;
            _posterIndexServices = posterIndexServices;
            _createServices = createServices;
            _context = context;
        }
        public IActionResult Elan(int id)
        {

            //ElanDetailViewModel elanDetail = new ElanDetailViewModel
            //{
            //    Poster = await _elanDetailIndexServices.GetPoster(id),
            //    SimilarPosters = await _elanDetailIndexServices.GetSimilarPosters(id),
            //};
            //return View(elanDetail);
            var elanDetail = _posterDetailVM(id);

            return View(elanDetail);
        }

        public IActionResult Axtaris(SearchDto searchDto)
        {
            
            var posterAll = _searchServices.SearchPosterAll(searchDto);
            var posterVip = _searchServices.SearchPosterVip(searchDto);

            ViewBag.CategoryId = searchDto.CategoryId;
            ViewBag.SubCategoryId = searchDto.SubCategoryId;
            ViewBag.CityId = searchDto.CityId;
            ViewBag.PosterName = searchDto.PosterName;
            ViewBag.PageIndex = searchDto.Page;
            SearchViewModel searchViewModel = new SearchViewModel
            {
                PagenetedItemsPreVip = PagenetedList<Poster>.CreateRandom(posterVip, searchDto.Page, 8),
                PagenetedItemsAll = PagenetedList<Poster>.Create(posterAll, searchDto.Page, 16),
            };
            return View(searchViewModel);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> PosterPayment(PaymentCreateDto paymentCreateDto)
        {
            var elanDetail = new ElanDetailViewModel();

            try
            {
                elanDetail = _posterDetailVM(paymentCreateDto.PosterId);
                await _paymentCreateServices.PaymentCheck(paymentCreateDto);
                await _paymentCreateServices.PaymentCreate(paymentCreateDto);

            }
            catch (NotFoundException ex)
            {

                ModelState.AddModelError("", ex.Message);
                //TempData["Error"] = ("Proses uğursuz oldu!");
                return View("Elan", elanDetail);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = ("Proses uğursuz oldu!");
                return View("Elan", elanDetail);

            }

            //return RedirectToAction("index", "anasehife");
            TempData["Error"] = ("Proses uğursuz oldu!");

            return View("Elan", elanDetail);

        }


        public IActionResult ConfirmPayment()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult ConfirmPayment(ConfirmViewModel confirm)
        {
            return Ok(confirm);
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
                _numberServices.PhoneNumberValidation(posterCreateDto.PhoneNumber);

                //sekil yoxlanilmasi
                _posterCreateValueCheckServices.ImageCheck(posterCreateDto.ImageFiles);


                //nomre filterlemesi
                posterCreateDto.PhoneNumber = _numberServices.PhoneNumberFilter(posterCreateDto.PhoneNumber);


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
                    var authentication = await _autenticationServices.CreateAuthentication(token, code, posterCreateDto.PhoneNumber);

                    //Link
                    url = Url.Action("NumberAuthentication", "elanlar", new { phoneNumber = posterCreateDto.PhoneNumber, token = token }, Request.Scheme);

                    //Cookie yaradilmasi ve seklin uploads papkasina yuklenmesi
                    _createServices.CreatePosterCookie(posterCreateDto.ImageFiles, posterCreateDto);
                    return Redirect(url);
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
                //imagescookie
                images = _createServices.GetImageFilesCookie();

                authentication = await _createServices.CheckAuthentication(code, phoneNumber, token, images);


                //postercookie
                posterCreateDto = _createServices.GetPosterCookie();


                //eger databsede user yoxdusa yeni  user yaradilmasi
                user = await _createServices.CreateNewUser(code, phoneNumber, posterCreateDto.Email, posterCreateDto.UserName);

                //feature entites yaradilmasi
                var feature = await _createServices.CreatePosterFeature(posterCreateDto);
                //poster entites yaradilmasi
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


        //Wishlisht
        public async Task<IActionResult> AddWishList(int id)
        {
            WishViewModel wishData = null;
            try
            {
                await _posterWishlistAddServices.IsPoster(id);
                var user = await _posterWishlistAddServices.IsAuthenticated();
                if (user != null && user.IsAdmin == false) await _posterWishlistAddServices.UserAddWish(id, user);
                else _posterWishlistAddServices.CookieAddWish(id);
            }
            catch (ItemNotFoundException ex)
            {
                TempData["Success"] = ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

            TempData["Success"] = "poster add wishlist";
            return Ok(wishData);
        }

        public async Task<IActionResult> DeleteWish(int id)
        {
            List<WishItemDto> wishItems = null;
            try
            {
                await _posterWishlistDeleteServices.IsPoster(id);
                var user = await _posterWishlistAddServices.IsAuthenticated();
                if (user != null && user.IsAdmin == false) await _posterWishlistDeleteServices.UserDeleteWish(id, user);
                else wishItems = _posterWishlistDeleteServices.CookieDeleteWish(id, wishItems);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            HttpContext.Response.Cookies.Append("wishItemList", JsonConvert.SerializeObject(wishItems));
            return Ok(wishItems);
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
        private ElanDetailViewModel _posterDetailVM(int id)
        {
            var poster = _context.Posters.Include(x => x.PosterFeatures)
                .Include(x => x.PosterUserIds).ThenInclude(x => x.AppUser)
                .Include(x => x.PosterFeatures).ThenInclude(x => x.City)
                .Include(x => x.PosterFeatures).ThenInclude(x => x.SubCategory)
                .Include(x => x.PosterFeatures).ThenInclude(x => x.SubCategory.Category)
                .Include(x => x.PosterImages)
                .Where(x => x.IsDelete == false)
                .FirstOrDefault(x => x.Id == id);
            if (poster == null) throw new Exception();

            var similarPoster = _context.Posters
                .Include(x => x.PosterFeatures)
                .Include(x => x.PosterImages)
                .Include(x => x.PosterImages)
                .Include(x => x.PosterFeatures)
                .ThenInclude(x => x.City)
                .Where(x => x.IsDelete == false && x.Id != id && x.PosterFeatures.SubCategory.CategoryId == poster.PosterFeatures.SubCategory.CategoryId).ToList();

            if (similarPoster == null) throw new Exception();


            var user = _context.PosterUserIds.Where(x => x.IsDelete == false).FirstOrDefault(x => x.PosterId == id);
            if (user == null)
                throw new Exception();
            ElanDetailViewModel elanDetail = new ElanDetailViewModel
            {
                Poster = poster,
                SimilarPosters = similarPoster,
                User = user,
                PaymentCreateDto = new PaymentCreateDto(),
                ServiceDurations = _context.ServiceDurations.Where(x => !x.IsDelete).ToList(),
            };
            return elanDetail;
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
