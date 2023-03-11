using Ferma.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Core.IUnitOfWork
{
    public interface IUnitOfWork
    {
        IPosterRepository PosterRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICityRepository CityRepository { get; }
        IContactUsRepository ContactUsRepository { get; }
        IImageSettingRepository ImageSettingRepository { get; }
        IPosterFeaturesRepository PosterFeaturesRepository { get; }
        IPosterImageRepository PosterImageRepository { get; }
        IPosterUserIdRepository PosterUserIdRepository { get; }
        ISettingRepository SettingRepository { get; }
        ISubCategoryRepository SubCategoryRepository { get; }
        IUserRepository UserRepository { get; }
        IWishItemRepository WishItemRepository { get; }
        IUserAuthenticationRepository UserAuthenticationRepository { get; }

        Task<int> CommitAsync();

    }
}
