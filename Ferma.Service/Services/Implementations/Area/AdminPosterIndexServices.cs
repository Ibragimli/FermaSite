using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Services.Interfaces.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferma.Service.Services.Implementations.Area
{
    public class AdminPosterIndexServices : IAdminPosterIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminPosterIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<Poster> GetPoster()
        {
            var poster = _unitOfWork.PosterRepository.asQueryablePoster();
            poster = poster.Where(x => !x.IsDelete);
            return poster;
        }
    }
}
