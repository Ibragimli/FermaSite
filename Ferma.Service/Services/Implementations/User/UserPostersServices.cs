using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferma.Service.Services.Implementations.User
{
    public class UserPostersServices : IUserPostersServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserPostersServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<Poster> AllPosters(string phoneNumber)
        {
            var poster = _unitOfWork.PosterRepository.asQueryablePoster().Where(x => x.PosterFeatures.PhoneNumber == phoneNumber);
            poster = poster.Where(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Active);

            return poster;
        }

        public IQueryable<Poster> VipPosters(string phoneNumber)
        {
            DateTime now = DateTime.UtcNow;
            var poster = _unitOfWork.PosterRepository.asQueryablePoster().Where(x => x.PosterFeatures.PhoneNumber == phoneNumber);
            poster = poster.Where(x => x.PosterFeatures.ExpirationDateVip > now);
            poster = poster.Where(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Active);
            return poster;
        }
        public IQueryable<Poster> PremiumPosters(string phoneNumber)
        {
            DateTime now = DateTime.UtcNow;
            var poster = _unitOfWork.PosterRepository.asQueryablePoster().Where(x => x.PosterFeatures.PhoneNumber == phoneNumber);
            poster = poster.Where(x => x.PosterFeatures.ExpirationDatePremium > now);
            poster = poster.Where(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Active);
            return poster;
        }
    }
}
