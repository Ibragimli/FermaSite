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
    public class CategoryController : Controller
    {
        private readonly IAdminCategoryServices _adminCategoryServices;

        public CategoryController(IAdminCategoryServices adminCategoryServices)
        {
            _adminCategoryServices = adminCategoryServices;
        }
        public IActionResult Index(int page = 1, string name = null)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            try
            {
                categoryViewModel = CategoryVM(page, categoryViewModel, name);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            ViewBag.CategoryName = name;
            return View(categoryViewModel);
        }
        public IActionResult Create()
        {
            var category = new Category();

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();

            Category oldCategory = new Category();
            try
            {
                categoryViewModel = CategoryVM(1, categoryViewModel, null);

                await _adminCategoryServices.CategoryCreate(category);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldCategory);
            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldCategory);
            }

            catch (ImageNullException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(oldCategory);
            }
            catch (ImageFormatException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(oldCategory);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return View("index", categoryViewModel);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = new Category();
            try
            {
                category = await _adminCategoryServices.GetCategory(id);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            Category oldCategory = new Category();
            try
            {
                oldCategory = await _adminCategoryServices.GetCategory(category.Id);
                await _adminCategoryServices.CategoryEdit(category);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldCategory);
            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldCategory);
            }

            catch (ImageNullException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(oldCategory);
            }
            catch (ImageFormatException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(oldCategory);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return View(oldCategory);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            try
            {
                categoryViewModel = CategoryVM(1, categoryViewModel, null);
                await _adminCategoryServices.CategoryDelete(id);
            }
            catch (ImageNullException ex)
            {
                TempData["Error"] = (ex.Message);
                return View(categoryViewModel);
            }
            catch (ItemAlreadyException ex)
            {
                TempData["Error"] = (ex.Message);
                return View(categoryViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Kategoriya silindi!");
            return Ok(categoryViewModel);
        }

        private CategoryViewModel CategoryVM(int page, CategoryViewModel categoryViewModel, string name)
        {
            var categories = _adminCategoryServices.GetCategories(name);
            categoryViewModel = new CategoryViewModel
            {
                Categories = PagenetedList<Category>.Create(categories, page, 3),
            };
            return categoryViewModel;
        }
    }
}
