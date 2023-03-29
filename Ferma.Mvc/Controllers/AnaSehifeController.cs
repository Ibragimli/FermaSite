using Ferma.Core.Entites;
using Ferma.Mvc.ViewModels;
using Ferma.Service.Helper;
using Ferma.Service.Services.Implementations.User;
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
        public async Task<IActionResult> Index(int page = 1)
        {
            var posterAll = _anaSehifeIndexServices.GetPostersAsync();
            var posterVip = _anaSehifeIndexServices.GetVipPosterAsync();
            var posterServices = _anaSehifeIndexServices.GetPrePosterAsync();
            var categories = await _anaSehifeIndexServices.GetAllCategoryAsync();

            AnaSehifeViewModel anaSehifeVM = new AnaSehifeViewModel
            {
                PagenatedItemsAll = PagenetedList<Poster>.Create(posterAll, page, 20),
                PagenatedItemsVip = PagenetedList<Poster>.CreateRandom(posterVip, page, 20),
                PagenatedItemsPremium = PagenetedList<Poster>.CreateRandom(posterServices, page, 20),
                Categories = categories,
            };
            return View(anaSehifeVM);
        }

    }

}
