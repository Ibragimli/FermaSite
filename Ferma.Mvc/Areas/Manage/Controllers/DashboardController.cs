using Ferma.Mvc.Areas.Manage.ViewModels;
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
    //[Authorize(Roles = "SuperAdmin,Admin")]

    public class DashboardController : Controller
    {
        private readonly IDashboardServices _dashboardServices;

        public DashboardController(IDashboardServices dashboardServices)
        {
            _dashboardServices = dashboardServices;
        }
        public async Task<IActionResult> Index()
        {
            var dashboardVM = await DashboardVM();
            return View(dashboardVM);
        }
        private async Task<DashboardViewModel> DashboardVM()
        {
            DashboardViewModel dashboardView = new DashboardViewModel
            {
                AllPosters = await _dashboardServices.AllPosterCount(),
                VipPosters = await _dashboardServices.VipPosterCount(),
                PremiumPosters = await _dashboardServices.PremiumPosterCount(),
                NewContact = await _dashboardServices.NewContactCount(),
                NewPosters = await _dashboardServices.NewPosterCount(),
                Money = await _dashboardServices.PaymentMoney(),
                Payments = await _dashboardServices.Payments(),
            };
            return dashboardView;
        }
    }
}
