using Ferma.Core.IUnitOfWork;
using Ferma.Core.Repositories;
using Ferma.Data.Datacontext;
using Ferma.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private ICategoryRepository _categoryRepository;
        private IPosterRepository _posterRepository;
        private ICityRepository _cityRepository;
        private IContactUsRepository _contactUsRepository;
        private IImageSettingRepository _imageSettingRepository;
        private IPosterFeaturesRepository _posterFeaturesRepository;
        private IPosterImageRepository _posterImageRepository;
        private IPosterUserIdRepository _posterUserIdRepository;
        private ISettingRepository _settingRepository;
        private ISubCategoryRepository _subCategoryRepository;
        private IUserRepository _userRepository;
        private IWishItemRepository _wishItemRepository;


        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IPosterRepository PosterRepository => _posterRepository = _posterRepository ?? new PosterRepository(_context);

        public ICityRepository CityRepository => _cityRepository = _cityRepository ?? new CityRepository(_context);

        public IContactUsRepository ContactUsRepository => _contactUsRepository = _contactUsRepository ?? new ContactUsRepository(_context);
        public IImageSettingRepository ImageSettingRepository => _imageSettingRepository = _imageSettingRepository ?? new ImageSettingRepository(_context);

        public IPosterFeaturesRepository PosterFeaturesRepository => _posterFeaturesRepository = _posterFeaturesRepository ?? new PosterFeaturesRepository(_context);

        public IPosterImageRepository PosterImageRepository => _posterImageRepository = _posterImageRepository ?? new PosterImageRepository(_context);

        //public IPosterUserIdRepository PosterUserIdRepository => _posterUserIdRepository = _posterUserIdRepository ?? new PosterappusIdRepository(_context);

        public ISettingRepository SettingRepository => _settingRepository = _settingRepository ?? new SettingRepository(_context);

        public ISubCategoryRepository SubCategoryRepository => _subCategoryRepository = _subCategoryRepository ?? new SubCategoryRepository(_context);

        public IUserRepository UserRepository => _userRepository = _userRepository ?? new UserRepository(_context);

        public IWishItemRepository WishItemRepository => _wishItemRepository = _wishItemRepository ?? new WishItemRepository(_context);

        public ICategoryRepository CategoryRepository => _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);

        public IPosterUserIdRepository PosterUserIdRepository => throw new NotImplementedException();

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
