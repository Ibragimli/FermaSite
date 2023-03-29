using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class PosterDetailIndexServices : IPosterDetailIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PosterDetailIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Poster> GetPoster(int id)
        {
            var poster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == id && !x.IsDelete, "PosterImages", "PosterUserIds.AppUser", "PosterFeatures.City", "PosterFeatures.SubCategory.Category");

            if (poster == null) throw new NotFoundException("error444");
            return poster;
        }
        public async Task<List<Poster>> GetSimilarPoster(int id, Poster poster)
        {
            var similarPoster = await _unitOfWork.PosterRepository.GetAllAsync(x => x.Id != id && !x.IsDelete && x.PosterFeatures.SubCategory.CategoryId == poster.PosterFeatures.SubCategory.CategoryId,
                   "PosterImages", "PosterUserIds.AppUser", "PosterFeatures.City", "PosterFeatures.SubCategory.Category");

            return similarPoster.ToList();
        }

        public async Task<PosterUserId> GetUser(int id)
        {
            var user = await _unitOfWork.PosterUserIdRepository.GetAsync(x => x.PosterId == id && !x.IsDelete);
            if (user == null) throw new NotFoundException("error444");

            return user;
        }

        public async Task<int> GetWishCount(int id)
        {
            var count = await _unitOfWork.WishItemRepository.GetTotalCountAsync(x => x.PosterId == id && !x.IsDelete);

            return count;
        }
        public async Task<List<ServiceDuration>> GetServiceDurations()
        {
            var durations = await _unitOfWork.ServiceDurationRepository.GetAllAsync(x => !x.IsDelete);
            return durations.ToList();
        }
        public async Task PosterViewCount(Poster poster)
        {
            poster.PosterFeatures.ViewCount++;
            await _unitOfWork.CommitAsync();
        }
    }
}
