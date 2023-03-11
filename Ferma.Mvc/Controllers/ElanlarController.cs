using Ferma.Core.Entites;
using Ferma.Data.Datacontext;
using Ferma.Mvc.ViewModels;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Ferma.Mvc.Controllers
{
    public class ElanlarController : Controller
    {
        private readonly IManageImageHelper _imageHelper;
        private readonly IPosterCreateIndexServices _posterIndexServices;
        private readonly IPosterCreateServices _createServices;
        private readonly DataContext _context;

        public ElanlarController(IManageImageHelper imageHelper, IPosterCreateIndexServices posterIndexServices, IPosterCreateServices createServices, DataContext context)
        {
            _imageHelper = imageHelper;
            _posterIndexServices = posterIndexServices;
            _createServices = createServices;
            _context = context;
        }
        public IActionResult YeniElan()
        {
            PosterCreateViewModels posterCreateView = new PosterCreateViewModels
            {
                PosterCreateDto = new PosterCreateDto(),
                Categories = _context.Categories.ToList(),
                SubCategories = _context.SubCategories.ToList(),
                Cities = _context.Cities.ToList(),
            };

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
            PosterCreateViewModels posterCreateView = new PosterCreateViewModels
            {
                PosterCreateDto = new PosterCreateDto(),
                Categories = _context.Categories.ToList(),
                SubCategories = _context.SubCategories.ToList(),
                Cities = _context.Cities.ToList(),
            };
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
                _createServices.SendCode(posterCreateDto.Email, code);

                //var url = _createServices.CreateUrl(posterCreateDto.Email);

                string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                var random = new Random();
                var token = new string(Enumerable.Repeat(chars, 30).Select(s => s[random.Next(s.Length)]).ToArray());
                url = Url.Action("NumberAuthentication", "elanlar", new { email = posterCreateDto.Email, token = token }, Request.Scheme);
                url = Url.Action("NumberAuthentication", "elanlar", new { email = posterCreateDto.Email, token = token }, Request.Scheme);

                Timer timer = new Timer();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                //TempData["Error"] = ("Proses uğursuz oldu!");
                return View(posterCreateView);
            }
            return Redirect(url);
        }
        public IActionResult NumberAuthentication(string email, string token)
        {
            PosterCreateViewModels posterCreateView = new PosterCreateViewModels
            {
                PosterCreateDto = new PosterCreateDto(),
                Categories = _context.Categories.ToList(),
                SubCategories = _context.SubCategories.ToList(),
                Cities = _context.Cities.ToList(),
            };


            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult NumberAuthentication(string code)
        {
            PosterCreateViewModels posterCreateView = new PosterCreateViewModels
            {
                PosterCreateDto = new PosterCreateDto(),
                Categories = _context.Categories.ToList(),
                SubCategories = _context.SubCategories.ToList(),
                Cities = _context.Cities.ToList(),
            };


            return View();
        }


    }
}
