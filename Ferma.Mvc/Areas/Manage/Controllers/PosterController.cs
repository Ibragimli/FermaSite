using Ferma.Core.Entites;
using Ferma.Mvc.Areas.Manage.ViewModels;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.Area;
using Ferma.Service.Helper;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class PosterController : Controller
    {
        private readonly IAdminPosterIndexServices _posterIndexServices;
        private readonly IPosterDeleteServices _posterDeleteServices;
        private readonly IAdminPosterDetailIndexServices _adminPosterDetailServices;
        private readonly IAdminPosterEditServices _adminPosterEditServices;

        public PosterController(IAdminPosterIndexServices posterIndexServices, IPosterDeleteServices posterDeleteServices, IAdminPosterEditServices adminPosterEditServices, IAdminPosterDetailIndexServices adminPosterDetailServices)
        {
            _posterIndexServices = posterIndexServices;
            _posterDeleteServices = posterDeleteServices;
            _adminPosterDetailServices = adminPosterDetailServices;
            _adminPosterEditServices = adminPosterEditServices;
        }
        public async Task<IActionResult> Index(int page = 1, string name = null, string phoneNumber = null, int subCategoryId = 0)
        {
            PosterIndexViewModel posterVM = new PosterIndexViewModel();
            try
            {
                var poster = _posterIndexServices.GetPoster(name, phoneNumber, subCategoryId);
                await _posterIndexServices.IsDisabled();
                posterVM = new PosterIndexViewModel
                {
                    Posters = PagenetedList<Poster>.Create(poster, page, 10),
                    Categories = await _adminPosterDetailServices.GetCategories(),
                    SubCategories = await _adminPosterDetailServices.GetSubCategories(),
                    PosterDeleteModal = new PosterDeleteModal(),

                };
            }
            catch (NotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            ViewBag.SubCategoryId = subCategoryId;
            ViewBag.PhoneNumber = phoneNumber;
            ViewBag.Name = name;
            return View(posterVM);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var posterVM = new AdminPosterEditViewModel();
            try
            {
                posterVM = await _detailVM(id);

            }
            catch (NotFoundException)
            {

                return RedirectToAction("index", "notfound");
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");

            }
            return View(posterVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Poster poster)
        {
            var posterVM = new AdminPosterEditViewModel();


            try
            {
                posterVM = await _detailVM(poster.Id);
                _adminPosterEditServices.CheckPostEdit(poster);
                await _adminPosterEditServices.EditPoster(poster);
            }

            catch (NotFoundException)
            {

                return RedirectToAction("index", "notfound");
            }
            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Detail", posterVM);

            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Proses uğurlu oldu");
            return View("Detail", posterVM);

        }

        public async Task<IActionResult> PosterAccept(int id)
        {
            var posterVM = new AdminPosterEditViewModel();


            try
            {
                posterVM = await _detailVM(id);
                await _adminPosterEditServices.PosterActive(id);
            }

            catch (NotFoundException)
            {

                return RedirectToAction("index", "notfound");
            }
            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Detail", posterVM);

            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Proses uğurlu oldu");
            return View("Detail", posterVM);
        }
        public async Task<IActionResult> PosterDisabled(int id)
        {
            var posterVM = new AdminPosterEditViewModel();


            try
            {
                posterVM = await _detailVM(id);
                await _adminPosterEditServices.PosterDisabled(id);
            }

            catch (NotFoundException)
            {

                return RedirectToAction("index", "notfound");
            }
            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Detail", posterVM);

            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Proses uğurlu oldu");
            return View("Detail", posterVM);
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _posterDeleteServices.DeletePoster(id);
            }
            catch (ItemNotFoundException ex)
            {
                TempData["Success"] = (ex.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Elan silindi!");
            return Ok();
        }

        private async Task<AdminPosterEditViewModel> _detailVM(int id)
        {
            var poster = await _adminPosterDetailServices.GetPoster(id);
            var user = await _adminPosterDetailServices.GetAppUser(id);
            AdminPosterEditViewModel posterVM = new AdminPosterEditViewModel
            {
                Poster = poster,
                SubCategories = await _adminPosterDetailServices.GetSubCategories(),
                Cities = await _adminPosterDetailServices.GetAllCity(),
                PosterEditPostDto = new PosterEditPostDto(),
                Categories = await _adminPosterDetailServices.GetCategories(),
                AppUser = user.AppUser,

            };
            return posterVM;
        }
    }
}
