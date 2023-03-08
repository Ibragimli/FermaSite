using Ferma.Mvc.ViewModels;
using Ferma.Service.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Controllers
{
    public class ElanlarController : Controller
    {
        public ElanlarController()
        {

        }
        public IActionResult YeniElan()
        {
            
            PosterCreateViewModels posterCreateView = new PosterCreateViewModels
            {
                PosterCreateDto = new PosterCreateDto(),
            };
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult YeniElan(PosterCreateDto posterCreateDto)
        {
            return View();
        }
    }
}
