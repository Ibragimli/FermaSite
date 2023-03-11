using Ferma.Core.Entites;
using Ferma.Data.Datacontext;
using Ferma.Mvc.ViewModels;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IManageImageHelper _imageHelper;
        private readonly IPosterCreateIndexServices _posterIndexServices;
        private readonly IPosterCreateServices _createServices;
        private readonly DataContext _context;

        public ElanlarController(UserManager<AppUser> userManager, IManageImageHelper imageHelper, IPosterCreateIndexServices posterIndexServices, IPosterCreateServices createServices, DataContext context)
        {
            _userManager = userManager;
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
            try
            {

                ////Check
                //_imageHelper.ImagesCheck(posterCreateDto.ImageFiles);


                //feature = await _createServices.CreatePosterFeature(posterCreateDto);
                //poster = await _createServices.CreatePoster(feature, posterCreateDto.ImageFiles);

                ////Create
                //_createServices.CreateImage(posterCreateDto.ImageFiles, poster.Id);
                var code = _createServices.AutenticationCodeCreate();
                //_createServices.SendCode(posterCreateDto.Email, code);

                //var url = _createServices.CreateUrl(posterCreateDto.Email);

                AppUser userExist = User.Identity.IsAuthenticated ? _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin) : null;
                if (userExist == null)
                {
                    string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    var random = new Random();
                    var token = new string(Enumerable.Repeat(chars, 30).Select(s => s[random.Next(s.Length)]).ToArray());
                    UserAuthentication authentication = new UserAuthentication
                    {
                        Code = code,
                        Token = token,
                        IsDisabled = false,
                        PhoneNumber = posterCreateDto.Email,
                    };
                    _context.Add(authentication);
                    _context.SaveChanges();

                    url = Url.Action("NumberAuthentication", "elanlar", new { email = posterCreateDto.Email, token = token }, Request.Scheme);
                    return Redirect(url);

                }
                else
                {
                    //Check
                    _imageHelper.ImagesCheck(posterCreateDto.ImageFiles);


                    feature = await _createServices.CreatePosterFeature(posterCreateDto);
                    poster = await _createServices.CreatePoster(feature, posterCreateDto.ImageFiles);

                    //Create
                    _createServices.CreateImage(posterCreateDto.ImageFiles, poster.Id);
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
        public IActionResult NumberAuthentication(string email, string token)
        {
            var authentication = _context.UserAuthentications.Where(x => x.IsDisabled == false).FirstOrDefault(x => x.Token == token);

            if (authentication == null)
                return RedirectToAction("NotFound");

            //_timer(authentication);

            var posterCreateView = _posterVM();

            var authenticationViewModel = _autenticaitonVM(authentication.Token, authentication.PhoneNumber);

            return View(authenticationViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult NumberAuthentication(string code, string email, string token)
        {
            var authentication = _context.UserAuthentications.Where(x => x.IsDisabled == false).FirstOrDefault(x => x.Code == code && x.Token == token);
            var authenticationViewModel = _autenticaitonVM(token, email);
            if (authentication == null)
            {
                ModelState.AddModelError("code", "Kod yanlışdır!");
                return View(authenticationViewModel);
            }
            authentication.IsDisabled = true;
            _context.SaveChanges();
            return RedirectToAction("yenielan");
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

        private void _timer(UserAuthentication user)
        {
            Timer timer = new Timer();
            timer.Interval = 10000;
            timer.Start();

            if (timer.Interval == 10000)
            {
                user.IsDisabled = true;
                timer.Stop();

            }
        }
    }
}
