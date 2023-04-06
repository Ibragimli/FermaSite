using Ferma.Core.Entites;
using Ferma.Mvc.Areas.Manage.ViewModels;
using Ferma.Service.CustomExceptions;
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
    public class CityController : Controller
    {

        private readonly IAdminCityServices _adminCityServices;

        public CityController(IAdminCityServices adminCityServices)
        {
            _adminCityServices = adminCityServices;
        }
        public IActionResult Index(int page = 1, string name = null)
        {
            CityViewModel cityViewModel = new CityViewModel();
            try
            {
                cityViewModel = CityVM(page, cityViewModel, name);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            ViewBag.CityName = name;
            return View(cityViewModel);
        }
        public IActionResult Create()
        {
            var city = new City();

            return View(city);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(City City)
        {
            CityViewModel cityViewModel = new CityViewModel();

            City oldCity = new City();
            try
            {
                cityViewModel = CityVM(1, cityViewModel, null);

                await _adminCityServices.CityCreate(City);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldCity);
            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldCity);
            }
            catch (ItemAlreadyException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldCity);
            }

            catch (ImageNullException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(oldCity);
            }
            catch (ImageFormatException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(oldCity);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return RedirectToAction("index", cityViewModel);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var City = new City();
            try
            {
                City = await _adminCityServices.GetCity(id);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            return View(City);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(City City)
        {
            City oldCity = new City();
            try
            {
                oldCity = await _adminCityServices.GetCity(City.Id);
                await _adminCityServices.CityEdit(City);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldCity);
            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldCity);
            }

            catch (ImageNullException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(oldCity);
            }
            catch (ItemAlreadyException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldCity);
            }
            catch (ImageFormatException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(oldCity);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return View(oldCity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CityViewModel CityViewModel = new CityViewModel();
            try
            {
                await _adminCityServices.CityDelete(id);
            }

            catch (ItemUseException ex)
            {
                TempData["Error"] = (ex.Message);
                return Ok();
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Şəhər silindi!");
            return Ok();

        }

        private CityViewModel CityVM(int page, CityViewModel CityViewModel, string name)
        {
            var cities = _adminCityServices.GetCities(name);
            CityViewModel = new CityViewModel
            {
                Cities = PagenetedList<City>.Create(cities, page, 8),
            };
            return CityViewModel;
        }
    }
}
