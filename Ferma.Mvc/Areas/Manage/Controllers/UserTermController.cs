using Ferma.Core.Entites;
using Ferma.Mvc.Areas.Manage.ViewModels;
using Ferma.Service.CustomExceptions;
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
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]

    public class UserTermController : Controller
    {

        private readonly IAdminUserTermServices _adminUserTermServices;

        public UserTermController(IAdminUserTermServices adminUserTermServices)
        {
            _adminUserTermServices = adminUserTermServices;
        }
        public IActionResult Index(int page = 1, string title = null)
        {
            UserTermViewModel UserTermViewModel = new UserTermViewModel();
            try
            {
                UserTermViewModel = UserTermVM(page, UserTermViewModel, title);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            ViewBag.UserTermTitle = title;
            return View(UserTermViewModel);
        }
        public IActionResult Create()
        {
            var userTerm = new UserTerm();

            return View(userTerm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserTerm userTerm)
        {
            UserTermViewModel UserTermViewModel = new UserTermViewModel();

            UserTerm oldUserTerm = new UserTerm();
            try
            {
                UserTermViewModel = UserTermVM(1, UserTermViewModel, null);

                await _adminUserTermServices.UserTermCreate(userTerm);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(oldUserTerm);
            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(oldUserTerm);
            }
            catch (ItemAlreadyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(oldUserTerm);
            }

            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return RedirectToAction("index", UserTermViewModel);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var userTerm = new UserTerm();
            try
            {
                userTerm = await _adminUserTermServices.GetUserTerm(id);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            return View(userTerm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserTerm userTerm)
        {
            UserTerm oldUserTerm = new UserTerm();
            try
            {
                oldUserTerm = await _adminUserTermServices.GetUserTerm(userTerm.Id);
                await _adminUserTermServices.UserTermEdit(userTerm);
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(oldUserTerm);
            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(oldUserTerm);
            }

     
            catch (ItemAlreadyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(oldUserTerm);
            }
       
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return View(oldUserTerm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            UserTermViewModel userTermViewModel = new UserTermViewModel();
            try
            {
                userTermViewModel = UserTermVM(1, userTermViewModel, null);
                await _adminUserTermServices.UserTermDelete(id);
            }
            catch (ImageNullException ex)
            {
                TempData["Error"] = (ex.Message);
                return View(userTermViewModel);
            }
            catch (ItemUseException ex)
            {
                TempData["Error"] = (ex.Message);
                return View(userTermViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("İstifadəçi qaydası silindi!");
            return Ok(userTermViewModel);
        }

        private UserTermViewModel UserTermVM(int page, UserTermViewModel userTermViewModel, string title)
        {
            var terms = _adminUserTermServices.GetUserTerms(title);
            userTermViewModel = new UserTermViewModel
            {
                UserTerms = PagenetedList<UserTerm>.Create(terms, page, 8),
            };
            return userTermViewModel;
        }
    }
}
