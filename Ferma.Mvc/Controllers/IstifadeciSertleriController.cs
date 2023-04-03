using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Controllers
{
    public class IstifadeciSertleriController : Controller
    {
        private readonly IUserTermIndexServices _userTermIndexServices;

        public IstifadeciSertleriController(IUserTermIndexServices userTermIndexServices)
        {
            _userTermIndexServices = userTermIndexServices;
        }
        public async Task<IActionResult> Index()
        {
            var terms = await _userTermIndexServices.UserTerms();
            return View(terms);
        }
    }
}
