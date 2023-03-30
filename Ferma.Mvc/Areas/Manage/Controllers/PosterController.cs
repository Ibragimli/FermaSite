using Ferma.Core.Entites;
using Ferma.Mvc.Areas.Manage.ViewModels;
using Ferma.Service.Dtos.Area;
using Ferma.Service.Helper;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Areas.Manage.Controllers
{
    [Area("manage")]
    //[Authorize(Roles = "SuperAdmin,Admin")]

    public class PosterController : Controller
    {
        private readonly IAdminPosterIndexServices _posterIndexServices;
        private readonly IAdminPosterEditServices _adminPosterEditServices;

        public PosterController(IAdminPosterIndexServices posterIndexServices, IAdminPosterEditServices adminPosterEditServices)
        {
            _posterIndexServices = posterIndexServices;
            _adminPosterEditServices = adminPosterEditServices;
        }
        public IActionResult Index(int page = 1)
        {
            var poster = _posterIndexServices.GetPoster();
            PosterIndexViewModel posterVM = new PosterIndexViewModel
            {
                Posters = PagenetedList<Poster>.Create(poster, page, 10)
            };
            return View(posterVM);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var poster = await _adminPosterEditServices.GetPoster(id);
            var user = await _adminPosterEditServices.GetAppUser(id);
            AdminPosterEditViewModel posterVM = new AdminPosterEditViewModel
            {
                Poster = poster,
                SubCategories = await _adminPosterEditServices.GetSubCategories(),
                Cities = await _adminPosterEditServices.GetAllCity(),
                PosterEditPostDto = new PosterEditPostDto(),
                Categories = await _adminPosterEditServices.GetCategories(),
                AppUser = user.AppUser,
                
            };

            return View(posterVM);
        }
    }
}
