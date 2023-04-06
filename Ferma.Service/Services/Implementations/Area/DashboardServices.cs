using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Services.Interfaces.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class DashboardServices : IDashboardServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task IsActive()
        {
            var now = DateTime.UtcNow.AddHours(4);
            var posterCheck = await _unitOfWork.PosterRepository.IsExistAsync(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Active && x.PosterFeatures.ExpirationDateActive < now, "PosterFeatures");
            if (posterCheck)
            {
                var posters = await _unitOfWork.PosterRepository.GetAllAsync(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Active && x.PosterFeatures.ExpirationDateActive < now, "PosterFeatures");
                foreach (var poster in posters)
                {
                    poster.PosterFeatures.PosterStatus = PosterStatus.DeActive;
                    await _unitOfWork.CommitAsync();
                }
            }
        }
        public async Task<int> AllPosterCount()
        {
            var count = await _unitOfWork.PosterRepository.GetTotalCountAsync(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Active, "PosterFeatures");
            return count;
        }


        public async Task<int> NewContactCount()
        {
            var now = DateTime.UtcNow.AddHours(4).AddDays(5);
            var count = await _unitOfWork.ContactUsRepository.GetTotalCountAsync(x => !x.IsDelete && x.CreatedDate < now);
            return count;
        }

        public async Task<int> NewPosterCount()
        {
            var count = await _unitOfWork.PosterRepository.GetTotalCountAsync(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Waiting, "PosterFeatures");
            return count;
        }

        public async Task<decimal?> PaymentMoney()
        {
            decimal? money = 0;
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync(x => !x.IsDelete && x.Source != Source.Balance);
            foreach (var item in payments)
            {

                money += item.Amount;
            }
            return money;
        }

        public async Task<List<Payment>> Payments()
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync(x => !x.IsDelete);
            return payments.Take(8).ToList();
        }

        public async Task<int> PremiumPosterCount()
        {
            var now = DateTime.UtcNow.AddHours(4);
            var count = await _unitOfWork.PosterRepository.GetTotalCountAsync(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Active && x.PosterFeatures.ExpirationDatePremium > now, "PosterFeatures");
            return count;
        }

        public async Task<int> VipPosterCount()
        {
            var now = DateTime.UtcNow.AddHours(4);
            var count = await _unitOfWork.PosterRepository.GetTotalCountAsync(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Active && x.PosterFeatures.ExpirationDateVip > now, "PosterFeatures");
            return count;
        }
    }
}
