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

    public class ServiceDurationController : Controller
    {
        private readonly IAdminServiceDurationServices _adminServiceDurationServices;

        public ServiceDurationController(IAdminServiceDurationServices adminServiceDurationServices)
        {
            _adminServiceDurationServices = adminServiceDurationServices;
        }
        public IActionResult Index(int page = 1)
        {
            ServiceDurationViewModel serviceDurationVM = new ServiceDurationViewModel();
            try
            {
                serviceDurationVM = ServiceDurationVM(page, serviceDurationVM);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            return View(serviceDurationVM);
        }
        public IActionResult Create()
        {
            var ServiceDuration = new ServiceDuration();

            return View(ServiceDuration);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceDuration ServiceDuration)
        {
            ServiceDurationViewModel ServiceDurationViewModel = new ServiceDurationViewModel();

            ServiceDuration oldServiceDuration = new ServiceDuration();
            try
            {
                ServiceDurationViewModel = ServiceDurationVM(1, ServiceDurationViewModel);

                await _adminServiceDurationServices.ServiceDurationCreate(ServiceDuration);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldServiceDuration);
            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("name", ex.Message);
                return View(oldServiceDuration);
            }

            catch (ImageNullException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(oldServiceDuration);
            }
            catch (ImageFormatException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(oldServiceDuration);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return View("index", ServiceDurationViewModel);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var serviceVM = new ServiceDurationViewModel();
            try
            {
                serviceVM = await ServiceDurationEditVM(1, id, serviceVM);
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            return View(serviceVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceDuration ServiceDuration)
        {
            ServiceDuration oldServiceDuration = new ServiceDuration();
            var serviceVM = new ServiceDurationViewModel();

            try
            {

                oldServiceDuration = await _adminServiceDurationServices.GetServiceDuration(ServiceDuration.Id);
                serviceVM = await ServiceDurationEditVM(1, oldServiceDuration.Id, serviceVM);
                await _adminServiceDurationServices.ServiceDurationEdit(ServiceDuration);

            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(serviceVM);
            }
            catch (ItemFormatException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(serviceVM);
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Dəyişikliklər uğurlu oldu!");
            return View(serviceVM);
        }

        private async Task<ServiceDurationViewModel> ServiceDurationEditVM(int page, int id, ServiceDurationViewModel ServiceDurationViewModel)
        {
            var serviceDurations = _adminServiceDurationServices.GetServiceDurations();
            ServiceDurationViewModel = new ServiceDurationViewModel
            {
                ServiceDurations = PagenetedList<ServiceDuration>.Create(serviceDurations, page, 8),
                ServiceDuration = await _adminServiceDurationServices.GetServiceDuration(id),
            };
            return ServiceDurationViewModel;
        }
        private ServiceDurationViewModel ServiceDurationVM(int page, ServiceDurationViewModel ServiceDurationViewModel)
        {
            var serviceDurations = _adminServiceDurationServices.GetServiceDurations();
            ServiceDurationViewModel = new ServiceDurationViewModel
            {
                ServiceDurations = PagenetedList<ServiceDuration>.Create(serviceDurations, page, 8),
            };
            return ServiceDurationViewModel;
        }
    }
}
