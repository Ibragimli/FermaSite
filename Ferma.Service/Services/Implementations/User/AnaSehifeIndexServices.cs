using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class AnaSehifeIndexServices : IAnaSehifeIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnaSehifeIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Category>> GetAllCategoryAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(x => !x.IsDelete);
            return categories.ToList();
        }

        public IQueryable<Poster> GetPostersAsync()
        {
            var posters = _unitOfWork.PosterRepository.asQueryablePoster();
            return posters;
        }
        public IQueryable<Poster> GetVipPosterAsync()
        {
            var now = DateTime.Now;
            var posters = _unitOfWork.PosterRepository.asQueryablePoster().Where(x => x.PosterFeatures.ExpirationDateVip > now);
            return posters;
        }
        public IQueryable<Poster> GetPremiumPosterAsync()
        {
            var now = DateTime.Now;
            var posters = _unitOfWork.PosterRepository.asQueryablePoster().Where(x => x.PosterFeatures.ExpirationDatePremium > now);
            return posters;
        }
        public IQueryable<Poster> GetPrePosterAsync()
        {
            var now = DateTime.Now;
            var posters = _unitOfWork.PosterRepository.asQueryablePoster().Where(x => x.PosterFeatures.ExpirationDatePremium > now || x.PosterFeatures.ExpirationDatePremium > now);
            return posters;
        }

    }
}
