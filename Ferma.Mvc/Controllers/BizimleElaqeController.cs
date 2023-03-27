using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Controllers
{
    public class BizimleElaqeController : Controller
    {
        private readonly IContactUsServices _contactUsServices;

        public BizimleElaqeController(IContactUsServices contactUsServices)
        {
            _contactUsServices = contactUsServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(ContactUsDto contactUsDto)
        {
            try
            {
                _contactUsServices.CheckContactUs(contactUsDto);
                await _contactUsServices.CreateContactUs(contactUsDto);
            }
            catch (ItemNotFoundException)
            {
                TempData["Error"] = ("Xəta baş verdi!");
                return RedirectToAction("index");
            }
            catch (Exception)
            {
                TempData["Error"] = ("Xəta baş verdi!");
                return RedirectToAction("index");
            }

            TempData["Success"] = ("Mesajınız göndərildi!");
            return RedirectToAction("index");

        }
    }
}
