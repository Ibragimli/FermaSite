using Ferma.Core.Entites;
using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{

    public interface IPosterWishlistDeleteServices
    {
        Task IsPoster(int id);
        Task<WishItem> UserDeleteWish(int id, AppUser user);
        List<WishItemDto> CookieDeleteWish(int id, List<WishItemDto> wishItems);
        Task<AppUser> IsAuthenticated();
    }
}
