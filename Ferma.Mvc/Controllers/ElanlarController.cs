using Ferma.Core.Entites;
using Ferma.Core.Enums;
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
        private readonly IPosterDetailIndexServices _posterDetailIndexServices;
        private readonly IUserPostersServices _userPostersServices;
        private readonly IPosterSearchServices _searchServices;
        private readonly IPosterWishlistAddServices _posterWishlistAddServices;
        private readonly IPosterWishlistDeleteServices _posterWishlistDeleteServices;
        private readonly IPhoneNumberServices _numberServices;
        private readonly IPaymentCreateServices _paymentCreateServices;
        private readonly IPosterCreateValueCheckServices _posterCreateValueCheckServices;
        private readonly IAuthenticationServices _autenticationServices;
        private readonly IManageImageHelper _imageHelper;
        private readonly IPosterCreateIndexServices _posterIndexServices;
        private readonly IPosterCreateServices _createServices;
        private readonly DataContext _context;

        public ElanlarController(UserManager<AppUser> userManager, IPosterDetailIndexServices posterDetailIndexServices, IUserPostersServices userPostersServices, IPosterSearchServices searchServices, IPosterWishlistAddServices posterWishlistAddServices, IPosterWishlistDeleteServices posterWishlistDeleteServices, IPhoneNumberServices numberServices, IPaymentCreateServices paymentCreateServices, IPosterCreateValueCheckServices posterCreateValueCheckServices, IAuthenticationServices autenticationServices, IManageImageHelper imageHelper, IPosterCreateIndexServices posterIndexServices, IPosterCreateServices createServices, DataContext context)
        {
            _userManager = userManager;
            _posterDetailIndexServices = posterDetailIndexServices;
            _userPostersServices = userPostersServices;
            _searchServices = searchServices;
            _posterWishlistAddServices = posterWishlistAddServices;
            _posterWishlistDeleteServices = posterWishlistDeleteServices;
            _numberServices = numberServices;
            _paymentCreateServices = paymentCreateServices;
            _posterCreateValueCheckServices = posterCreateValueCheckServices;
            _autenticationServices = autenticationServices;
            _imageHelper = imageHelper;
            _posterIndexServices = posterIndexServices;
            _createServices = createServices;
            _context = context;
        }
        public async Task<IActionResult> Elan(int id)
        {


            var elanDetail = new ElanDetailViewModel();
            try
            {
                elanDetail = await _posterDetailVM(id);
                await _posterDetailIndexServices.PosterViewCount(elanDetail.Poster);
            }
            catch (NotFoundException)
            {
                return RedirectToAction("index", "notfound");

            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            return View(elanDetail);

        }

        public IActionResult IstifadecininElanlari(string phoneNumber, int page = 1)
        {
            var posterAll = _userPostersServices.AllPosters(phoneNumber);
            var posterVip = _userPostersServices.VipPosters(phoneNumber);
            var posterPremium = _userPostersServices.PremiumPosters(phoneNumber);
            IstifadeciElanViewModel elanViewModel = new IstifadeciElanViewModel
            {
                PosterVip = PagenetedList<Poster>.CreateRandom(posterVip, page, 8),
                PosterPremium = PagenetedList<Poster>.CreateRandom(posterPremium, page, 8),
                PosterAll = PagenetedList<Poster>.Create(posterAll, page, 20),
            };
            return View(elanViewModel);

        }


        public IActionResult Axtaris(SearchDto searchDto)
        {
            var posterAll = _searchServices.SearchPosterAll(searchDto);
            var posterVip = _searchServices.SearchPosterVip(searchDto);
            var posterPremium = _searchServices.SearchPosterPremium(searchDto);

            ViewBag.CategoryId = searchDto.CategoryId;
            ViewBag.SubCategoryId = searchDto.SubCategoryId;
            ViewBag.CityId = searchDto.CityId;
            ViewBag.PosterName = searchDto.PosterName;
            ViewBag.PageIndex = searchDto.Page;
            SearchViewModel searchViewModel = new SearchViewModel
            {
                PagenetedItemsVip = PagenetedList<Poster>.CreateRandom(posterVip, searchDto.Page, 8),
                PagenetedItemsPremium = PagenetedList<Poster>.CreateRandom(posterPremium, searchDto.Page, 8),
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
                elanDetail = await _posterDetailVM(paymentCreateDto.PosterId);
                await _paymentCreateServices.PaymentCheck(paymentCreateDto);
                if (paymentCreateDto.Source == Source.BankCard)
                {
                    var posterStr = JsonConvert.SerializeObject(paymentCreateDto);

                    HttpContext.Response.Cookies.Append("paymentDto", posterStr);
                }
                else
                {
                    await _paymentCreateServices.PaymentCreateBalance(paymentCreateDto);

                }

            }
            catch (NotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (PaymentValueException ex)
            {

                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = (ex.Message);
                return View("Elan", elanDetail);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = ("Proses uğursuz oldu!");
                return View("Elan", elanDetail);
            }

            //return RedirectToAction("index", "anasehife");
            if (paymentCreateDto.Source == Source.BankCard)
                return RedirectToAction("ConfirmPayment", "elanlar");
            else
            {
                TempData["Success"] = ("Proses uğurlu oldu");
                return View("Elan", elanDetail);
            }

        }
        public IActionResult ConfirmPayment()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(ConfirmViewModel confirm)
        {
            PaymentCreateDto paymentCreateDto = new PaymentCreateDto();
            //cookie
            string posterItem = HttpContext.Request.Cookies["paymentDto"];

            if (posterItem != null)
                paymentCreateDto = JsonConvert.DeserializeObject<PaymentCreateDto>(posterItem);
            else
                throw new CookieNotActiveException("Cookie-nizi aktiv edin!");
            try
            {
                if (confirm.Value == 1)
                {
                    if (paymentCreateDto.Services == PaymentService.PosterPayment)
                    {
                        await _paymentCreateServices.PaymentCreateBankCard(paymentCreateDto);
                    }
                }
                else
                    return RedirectToAction("elan", "elanlar", new { id = paymentCreateDto.PosterId });
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("elan", "elanlar", new { id = paymentCreateDto.PosterId });
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
            catch (CookieNotActiveException ex)
            {
                ModelState.AddModelError("Code", ex.Message);
                return View(authenticationViewModel);
            }
            catch (Exception)
            {

                ModelState.AddModelError("code", "Error");
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

            TempData["Success"] = "Elan sevimlilərə əlavə edildi";
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
        private async Task<ElanDetailViewModel> _posterDetailVM(int id)
        {
            var poster = await _posterDetailIndexServices.GetPoster(id);
            var similarPoster = await _posterDetailIndexServices.GetSimilarPoster(id, poster);
            ElanDetailViewModel elanDetail = new ElanDetailViewModel
            {
                Poster = poster,
                SimilarPosters = similarPoster,
                User = await _posterDetailIndexServices.GetUser(id),
                PaymentCreateDto = new PaymentCreateDto(),
                ServiceDurations = await _posterDetailIndexServices.GetServiceDurations(),
                WishCount = await _posterDetailIndexServices.GetWishCount(id),
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
