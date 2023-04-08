using Ferma.Core.Entites;
using Ferma.Mvc.Areas.Manage.ViewModels;
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
    public class PosterPaymentsController : Controller
    {
        private readonly IPosterPaymentIndexServices _posterPaymentIndexServices;

        public PosterPaymentsController(IPosterPaymentIndexServices posterPaymentIndexServices)
        {
            _posterPaymentIndexServices = posterPaymentIndexServices;
        }
        public IActionResult Index(int page = 1, int year = 0, int month = 0)
        {
            PosterPaymentViewModel posterPaymentViewModel = new PosterPaymentViewModel();
            try
            {
                var payments = _posterPaymentIndexServices.GetPayments(year, month);
                posterPaymentViewModel = new PosterPaymentViewModel
                {
                    Payments = PagenetedList<Payment>.Create(payments, page, 3),
                };
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            ViewBag.PPaymentsYear = year;
            ViewBag.PPaymentsMonth = month;
            return View(posterPaymentViewModel);
        }
    }
}
