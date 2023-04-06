using Ferma.Core.Entites;
using Ferma.Data.Datacontext;
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

    public class SettingController : Controller
    {
        private readonly DataContext _context;
        private readonly ISettingEditServices _SettingEditServices;
        private readonly ISettingIndexServices _SettingIndexServices;

        public SettingController(DataContext context, ISettingEditServices SettingEditServices, ISettingIndexServices SettingIndexServices)
        {
            _context = context;
            _SettingEditServices = SettingEditServices;
            _SettingIndexServices = SettingIndexServices;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            ViewBag.Page = page;

            var Settings = _SettingIndexServices.SearchCheck(search);

            SettingIndexViewModel SettingIndexVM = new SettingIndexViewModel
            {
                PagenatedItems = PagenetedList<Setting>.Create(Settings, page, 6),
            };

            return View(SettingIndexVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                await _SettingEditServices.IsExists(id);
                await _SettingEditServices.GetSearch(id);
            }
            catch (Exception)
            {
                return RedirectToAction("notfound", "error");
            }

            return View(await _SettingEditServices.GetSearch(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingEditDto SettingEdit)
        {
            try
            {
                await _SettingEditServices.SettingEdit(SettingEdit);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View(SettingEdit);
            }
            TempData["Success"] = ("Proses uğurlu oldu!");
            return RedirectToAction("index", "Setting");
        }

    }
}
