using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class ElanDetailIndexServices : IElanDetailIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ElanDetailIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Poster> GetPoster(int id)
        {
            var existPoster = await _unitOfWork.PosterRepository.IsExistAsync(x => x.Id == id);
            if (!existPoster)
                throw new ItemNotFoundException("Not Found!");
            string[] includes = { "PosterFeatures", "PosterImages" };

            var poster = await _unitOfWork.PosterRepository.GetAsync(x=>x.Id == id, includes);
            //var poster =  posterQueryable.FirstOrDefault(x => x.Id == id);
            return poster;
        }

        public async Task<List<Poster>> GetSimilarPosters(int id)
        {
            var existPoster = await _unitOfWork.PosterRepository.IsExistAsync(x => x.Id == id);
            if (!existPoster)
                throw new ItemNotFoundException("Not Found!");
            var poster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == id);

            var categoryPosters = await _unitOfWork.PosterRepository.GetAllAsync(x => x.PosterFeatures.SubCategoryId == poster.PosterFeatures.SubCategoryId);
            var posters = categoryPosters.Where(x => x.PosterFeatures.SubCategory.CategoryId == poster.PosterFeatures.SubCategory.CategoryId).ToList();
            return posters;
        }
    }
}
