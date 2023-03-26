using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferma.Service.Services.Implementations.User
{
    public class PosterSearchServices : IPosterSearchServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PosterSearchServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<Poster> SearchPosterAll(SearchDto searchDto)
        {
            var poster = _unitOfWork.PosterRepository.asQueryablePoster();
            if (searchDto.CategoryId != null)
                poster = poster.Where(x => x.PosterFeatures.SubCategory.CategoryId == searchDto.CategoryId);

            if (searchDto.SubCategoryId != null)
                poster = poster.Where(x => x.PosterFeatures.SubCategoryId == searchDto.SubCategoryId);

            if (searchDto.CityId != null)
                poster = poster.Where(x => x.PosterFeatures.CityId == searchDto.CityId);

            if (searchDto.PosterName != null)
                poster = poster.Where(i => EF.Functions.Like(i.PosterFeatures.Name, $"%{searchDto.PosterName}%"));

            return poster;
        }
        public IQueryable<Poster> SearchPosterVip(SearchDto searchDto)
        {
            var poster = _unitOfWork.PosterRepository.asQueryablePoster();
            if (searchDto.CategoryId != null)
                poster = poster.Where(x => x.PosterFeatures.SubCategory.CategoryId == searchDto.CategoryId);

            if (searchDto.SubCategoryId != null)
                poster = poster.Where(x => x.PosterFeatures.SubCategoryId == searchDto.SubCategoryId);

            if (searchDto.CityId != null)
                poster = poster.Where(x => x.PosterFeatures.CityId == searchDto.CityId);

            if (searchDto.PosterName != null)
                poster = poster.Where(i => EF.Functions.Like(i.PosterFeatures.Name, $"%{searchDto.PosterName}%"));

            poster.Where(x => x.PosterFeatures.IsPremium);
            poster.Where(x => x.PosterFeatures.IsVip);
            return poster;
        }
    }
}
