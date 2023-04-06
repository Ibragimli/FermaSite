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

    public class SubCategoryController : Controller
    {
        private readonly IAdminSubCategoryServices _adminSubCategoryServices;

        public SubCategoryController(IAdminSubCategoryServices adminSubCategoryServices)
        {
            _adminSubCategoryServices = adminSubCategoryServices;
        }
        public async Task<IActionResult> Index(int page = 1, string category = null, string subCategory = null)
        {
            SubCategoryViewModel SubCategoryViewModel = new SubCategoryViewModel();
            try
            {
                SubCategoryViewModel = await SubCategoryVM(page, SubCategoryViewModel, category, subCategory);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            ViewBag.SubCategoryName = subCategory;
            ViewBag.CategoryName = category;
            return View(SubCategoryViewModel);
        }
        public async Task<IActionResult> Create()
        {
            SubCategoryViewModel subCategory = new SubCategoryViewModel
            {
                Categories = await _adminSubCategoryServices.GetCategories(),
                SubCategory = new SubCategory(),
            };
            return View(subCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryViewModel subCategoryViewModel)
        {
            SubCategoryViewModel subCategory = new SubCategoryViewModel();

            try
            {
                subCategory = await SubCategoryVM(1, subCategory, null, null);

                await _adminSubCategoryServices.SubCategoryCreate(subCategoryViewModel.SubCategory);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(subCategory);
            }
            catch (ItemAlreadyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(subCategory);
            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(subCategory);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return View("index", subCategory);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var subCategoryVM = new SubCategoryViewModel();
            var subCategory = new SubCategory();
            try
            {
                subCategory = await _adminSubCategoryServices.GetSubCategory(id);
                subCategoryVM = await SubCategoryEditVM(1, subCategoryVM, id, null, null);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            return View(subCategoryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategory subCategory)
        {
            //SubCategory oldSubCategory = new SubCategory();
            var subCategoryVM = new SubCategoryViewModel();
            try
            {
                //oldSubCategory = await _adminSubCategoryServices.GetSubCategory(subCategory.Id);

                subCategoryVM = await SubCategoryEditVM(1, subCategoryVM, subCategory.Id, null, null);

                await _adminSubCategoryServices.SubCategoryEdit(subCategory);
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(subCategoryVM);
            }
            catch (ItemAlreadyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(subCategoryVM);
            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(subCategoryVM);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return View(subCategoryVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _adminSubCategoryServices.SubCategoryDelete(id);
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
            TempData["Success"] = ("AltKategoriya silindi!");
            return Ok();

        }

        private async Task<SubCategoryViewModel> SubCategoryEditVM(int page, SubCategoryViewModel SubCategoryViewModel, int id, string category, string subCategory)
        {
            var cities = _adminSubCategoryServices.GetSubCategorys(category, subCategory);
            SubCategoryViewModel = new SubCategoryViewModel
            {
                SubCategories = PagenetedList<SubCategory>.Create(cities, page, 3),
                Categories = await _adminSubCategoryServices.GetCategories(),
                SubCategory = await _adminSubCategoryServices.GetSubCategory(id),
            };
            return SubCategoryViewModel;
        }
        private async Task<SubCategoryViewModel> SubCategoryVM(int page, SubCategoryViewModel SubCategoryViewModel, string category, string subCategory)
        {
            var cities = _adminSubCategoryServices.GetSubCategorys(category, subCategory);
            SubCategoryViewModel = new SubCategoryViewModel
            {
                SubCategories = PagenetedList<SubCategory>.Create(cities, page, 8),
                Categories = await _adminSubCategoryServices.GetCategories(),
                SubCategory = new SubCategory(),
            };
            return SubCategoryViewModel;
        }
    }
}
