using Ferma.Core.Entites;
using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IPosterWishlistAddServices
    {
        Task IsPoster(int id);
        Task<WishPosterCreateDto> UserAddWish(int id, AppUser user);
        void CookieAddWish(int id);
        Task<AppUser> IsAuthenticated();
    }
}
