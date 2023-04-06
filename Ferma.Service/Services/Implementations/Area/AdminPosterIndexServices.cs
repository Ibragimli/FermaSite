using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class AdminPosterIndexServices : IAdminPosterIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminPosterIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task IsDisabled()
        {
            var now = DateTime.UtcNow.AddHours(4);
            var posterCheck = await _unitOfWork.PosterRepository.IsExistAsync(x => !x.IsDelete && !x.PosterFeatures.IsDisabled && x.PosterFeatures.ExpirationDateDisabled < now, "PosterFeatures");
            if (posterCheck)
            {
                var posters = await _unitOfWork.PosterRepository.GetAllAsync(x => !x.IsDelete && !x.PosterFeatures.IsDisabled && x.PosterFeatures.ExpirationDateDisabled < now, "PosterFeatures");
                foreach (var poster in posters)
                {
                    poster.PosterFeatures.PosterStatus = PosterStatus.Disabled;
                    poster.PosterFeatures.IsDisabled = true;
                    await _unitOfWork.CommitAsync();
                }
            }
        }
        public IQueryable<Poster> GetPoster(string name, string phoneNumber, int subCategoryId)
        {
            var poster = _unitOfWork.PosterRepository.asQueryablePoster();
            poster = poster.Where(x => !x.IsDelete);
            if (subCategoryId != 0)
                poster = poster.Where(x => x.PosterFeatures.SubCategoryId == subCategoryId);
            if (name != null)
                poster = poster.Where(i => EF.Functions.Like(i.PosterFeatures.Name, $"%{name}%"));
            if (phoneNumber != null)
                poster = poster.Where(i => EF.Functions.Like(i.PosterFeatures.PhoneNumber, $"%{phoneNumber}%"));

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


    }
}