using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class PosterWishlistAddServices : IPosterWishlistAddServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public PosterWishlistAddServices(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) : base()
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public void CookieAddWish(int id)
        {
            List<CookieWishItemDto> wishItems = new List<CookieWishItemDto>();
            string existWishItem = _httpContextAccessor.HttpContext.Request.Cookies["wishItemList"];
            if (existWishItem != null)
            {
                wishItems = JsonConvert.DeserializeObject<List<CookieWishItemDto>>(existWishItem);
            }
            CookieWishItemDto item = wishItems.Find(x => x.PosterId == id);
          
            if (item == null)
            {
                item = new CookieWishItemDto
                {
                    PosterId = id,
                };
                wishItems.Add(item);
            }

            var PosterIdStr = JsonConvert.SerializeObject(wishItems);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("wishItemList", PosterIdStr);
            var wishData = _getCookieWishItems(wishItems);
        }
        public async Task<AppUser> IsAuthenticated()
        {
            AppUser user = null;
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            }
            return user;
        }
        public async Task IsPoster(int id)
        {
            if (!await _unitOfWork.PosterRepository.IsExistAsync(x => x.Id == id))
            {
                throw new ItemNotFoundException("Mehsul Tapilmadi");
            }
            if (await _unitOfWork.PosterRepository.IsExistAsync(x => x.PosterFeatures.PosterStatus != PosterStatus.Active && x.Id == id))
            {
                throw new ItemFormatException("Elan aktiv deyil!");
            }
        }
        public async Task<WishPosterCreateDto> UserAddWish(int id, AppUser user)
        {
            WishItem wishItem = await _unitOfWork.WishItemRepository.GetAsync(x => x.AppUserId == user.Id && x.PosterId == id, "Poster", "Poster.PosterFeatures");

            if (wishItem == null)
            {
                wishItem = new WishItem
                {
                    AppUserId = user.Id,
                    PosterId = id,
                };
                await _unitOfWork.WishItemRepository.InsertAsync(wishItem);
                await _unitOfWork.CommitAsync();
            }


            var wishData = _getUserWishItems(await _unitOfWork.WishItemRepository.GetAllAsync(x => x.AppUserId == user.Id));
            return wishData;
        }
        private async Task<WishPosterCreateDto> _getCookieWishItems(List<CookieWishItemDto> cookieWishItems)
        {
            WishPosterCreateDto wishItems = new WishPosterCreateDto()
            {
                WishItems = new List<WishItemsDto>(),
            };

            foreach (var item in cookieWishItems)
            {

                var poster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == item.PosterId, "PosterFeatures");
                WishItemsDto wishItem = new WishItemsDto
                {
                    Name = poster.PosterFeatures.Name,
                    Price = (decimal)poster.PosterFeatures.Price,
                    PosterId = poster.Id,
                };
            }
            return wishItems;
        }
        private WishPosterCreateDto _getUserWishItems(IEnumerable<WishItem> wishItems)
        {
            WishPosterCreateDto wish = new WishPosterCreateDto
            {
                WishItems = new List<WishItemsDto>(),
            };
            foreach (var item in wishItems)
            {
                WishItemsDto wishItem = new WishItemsDto
                {
                    Name = item.Poster.PosterFeatures.Name,
                    Price = (decimal)item.Poster.PosterFeatures.Price,
                    PosterId = item.Poster.Id,
                };
                wish.WishItems.Add(wishItem);
            }
            return wish;
        }

    }
}
