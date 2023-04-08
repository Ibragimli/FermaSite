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
    public class BalancePaymentsController : Controller
    {
        private readonly IBalancePaymentIndexServices _balancePaymentIndexServices;

        public BalancePaymentsController(IBalancePaymentIndexServices balancePaymentIndexServices)
        {
            _balancePaymentIndexServices = balancePaymentIndexServices;
        }
        public IActionResult Index(int page = 1, int year = 0, int month = 0)
        {
            BalancePaymentViewModel balancePaymentViewModel = new BalancePaymentViewModel();
            try
            {
                var payments = _balancePaymentIndexServices.GetPayments(year, month);
                balancePaymentViewModel = new BalancePaymentViewModel
                {
                    Payments = PagenetedList<Payment>.Create(payments, page, 3),
                };
            }
            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            ViewBag.BPaymentsYear = year;
            ViewBag.BPaymentsMonth = month;
            return View(balancePaymentViewModel);
        }
    }
}
