using Ferma.Core.Entites;
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
    public class PosterWishlistDeleteServices : IPosterWishlistDeleteServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public PosterWishlistDeleteServices(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) : base()
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public List<WishItemDto> CookieDeleteWish(int id, List<WishItemDto> wishItems)
        {
            string wish = _httpContextAccessor.HttpContext.Request.Cookies["wishItemList"];
            wishItems = JsonConvert.DeserializeObject<List<WishItemDto>>(wish);
            WishItemDto PosterWish = wishItems.Find(x => x.PosterId == id);
            if (PosterWish == null) throw new ItemNotFoundException("Mehsul Tapilmadi");

            wishItems.Remove(PosterWish);

            return wishItems;
        }

        public async Task<AppUser> IsAuthenticated()
        {
            AppUser user = null;
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            return user;
        }
        public async Task IsPoster(int id)
        {
            if (!await _unitOfWork.PosterRepository.IsExistAsync(x => x.Id == id))
                throw new ItemNotFoundException("Mehsul Tapilmadi");
        }

        public async Task<WishItem> UserDeleteWish(int id, AppUser user)
        {
            WishItem wishItem = await _unitOfWork.WishItemRepository.GetAsync(x => x.PosterId == id);
            if (wishItem == null)
                throw new ItemNotFoundException("Mehsul Tapilmadi");

            _unitOfWork.WishItemRepository.Remove(wishItem);
            await _unitOfWork.CommitAsync();
            return wishItem;
        }
    }
}
