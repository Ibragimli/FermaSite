using Ferma.Core.IUnitOfWork;
using Ferma.Core.Repositories;
using Ferma.Data.Datacontext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;



        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IPosterRepository PosterRepository => throw new NotImplementedException();

        public ICityRepository CityRepository => throw new NotImplementedException();

        public IContactUsRepository ContactUsRepository => throw new NotImplementedException();

        public IImageSettingRepository ImageSettingRepository => throw new NotImplementedException();

        public IPosterFeaturesRepository PosterFeaturesRepository => throw new NotImplementedException();

        public IPosterImageRepository PosterImageRepository => throw new NotImplementedException();

        public IPosterUserIdRepository PosterUserIdRepository => throw new NotImplementedException();

        public ISettingRepository SettingRepository => throw new NotImplementedException();

        public ISubCategoryRepository SubCategoryRepository => throw new NotImplementedException();

        public IUserRepository UserRepository => throw new NotImplementedException();

        public IWishItemRepository WishItemRepository => throw new NotImplementedException();

        public ICategoryRepository CategoryRepository => throw new NotImplementedException();

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
