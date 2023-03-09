using Ferma.Core.Entites;
using Ferma.Data.Datacontext;
using Ferma.Mvc.ViewModels;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult NumberAuthentication()
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
            try
            {

                //Check
                _imageHelper.ImagesCheck(posterCreateDto.ImageFiles);


                feature = await _createServices.CreatePosterFeature(posterCreateDto);
                poster = await _createServices.CreatePoster(feature, posterCreateDto.ImageFiles);

                //Create
                _createServices.CreateImage(posterCreateDto.ImageFiles, poster.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                //TempData["Error"] = ("Proses uğursuz oldu!");
                return View(posterCreateView);
            }
            return RedirectToAction("NumberAuthentication", "elanlar");
        }
    }
}
