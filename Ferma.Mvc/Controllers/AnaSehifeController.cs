using Ferma.Mvc.ViewModels;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Controllers
{
    public class AnaSehifeController : Controller
    {
        private readonly IAnaSehifeIndexServices _anaSehifeIndexServices;

        public AnaSehifeController(IAnaSehifeIndexServices anaSehifeIndexServices)
        {
            _anaSehifeIndexServices = anaSehifeIndexServices;
        }
        public IActionResult Index()
        {
            AnaSehifeViewModel anaSehifeVM = new AnaSehifeViewModel
            {
                Posters = _anaSehifeIndexServices.GetPostersAsync()
            };
            return View(anaSehifeVM);
        }

    }

}
