using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Services.Interfaces.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class AdminPosterDetailIndexServices : IAdminPosterDetailIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminPosterDetailIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Poster> GetPoster(int id)
        {
            var poster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == id, "PosterFeatures.SubCategory", "PosterFeatures.SubCategory.Category", "PosterFeatures.City", "PosterImages");
            if (poster == null)
                throw new NotFoundException("Error");
            return poster;
        }

        public async Task<List<SubCategory>> GetSubCategories()
        {
            var subCategories = await _unitOfWork.SubCategoryRepository.GetAllAsync(x => !x.IsDelete);
            if (subCategories == null)
                throw new NotFoundException("Error");
            return subCategories.ToList();
        }
        public async Task<List<Category>> GetCategories()
        {
            var subCategories = await _unitOfWork.CategoryRepository.GetAllAsync(x => !x.IsDelete);
            if (subCategories == null)
                throw new NotFoundException("Error");
            return subCategories.ToList();
        }
        public async Task<PosterUserId> GetAppUser(int posterId)
        {
            var user = await _unitOfWork.PosterUserIdRepository.GetAsync(x => !x.IsDelete && x.PosterId == posterId,"AppUser");
            if (user == null)
                throw new NotFoundException("Error");
            return user;
        }

        public async Task<List<City>> GetAllCity()
        {
            var cities = await _unitOfWork.CityRepository.GetAllAsync(x => !x.IsDelete);
            if (cities == null)
                throw new NotFoundException("Error");
            return cities.ToList();
        }
    }
}
