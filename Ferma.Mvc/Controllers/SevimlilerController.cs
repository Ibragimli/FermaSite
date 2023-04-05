using Ferma.Core.Entites;
using Ferma.Data.Datacontext;
using Ferma.Mvc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Controllers
{
    public class SevimlilerController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SevimlilerController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            WishListViewModel wishListVM = new WishListViewModel
            {
                WishlistItems = await _getWishItems(),
            };
            return View(wishListVM);

        }

        private async Task<List<WishlistItemViewModel>> _getWishItems()
        {
            List<WishlistItemViewModel> wishlistItems = new List<WishlistItemViewModel>();

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {
                List<WishItem> wishItems = _context.WishItems.Include(x => x.Poster).ThenInclude(x => x.PosterImages)
                    .Include(x => x.Poster).ThenInclude(x => x.PosterFeatures)
                    .Include(x => x.Poster).ThenInclude(x => x.PosterFeatures.City)
                    .Where(x => x.AppUserId == user.Id).ToList();

                foreach (var item in wishItems)
                {
                    WishlistItemViewModel wishlistItem = new WishlistItemViewModel
                    {
                        Poster = item.Poster,

                    };
                    wishlistItems.Add(wishlistItem);
                }
            }
            else
            {
                string wishItemsStr = HttpContext.Request.Cookies["wishItemList"];
                if (wishItemsStr != null)
                {
                    List<CookieWishItemViewModel> cookieWishItems = JsonConvert.DeserializeObject<List<CookieWishItemViewModel>>(wishItemsStr);

                    foreach (var item in cookieWishItems)
                    {
                        if (_context.Posters.Any(x => x.Id == item.PosterId))
                        {
                            WishlistItemViewModel checkoutItem = new WishlistItemViewModel
                            {
                                Poster = _context.Posters.Include(x => x.PosterFeatures).Include(x => x.PosterImages).Include(x => x.PosterFeatures.City).FirstOrDefault(x => x.Id == item.PosterId),
                            };
                            wishlistItems.Add(checkoutItem);
                        }
                       
                    }
                }

            }

            return wishlistItems;
        }
    }
}
