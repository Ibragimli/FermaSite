using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class PaymentCreateServices : IPaymentCreateServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentCreateServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task PaymentCheck(PaymentCreateDto paymentCreateDto)
        {

            bool posterCheck = await _unitOfWork.PosterRepository.IsExistAsync(x => x.Id == paymentCreateDto.PosterId);
            if (!posterCheck)
                throw new NotFoundException("Notfound");
            if (paymentCreateDto.AppUserId != null)
            {
                bool userCheck = await _unitOfWork.UserRepository.IsExistAsync(x => x.Id == paymentCreateDto.AppUserId);
                if (!userCheck)
                    throw new NotFoundException("Notfound");
            }
            bool durationCheck = await _unitOfWork.ServiceDurationRepository.IsExistAsync(x => x.Id == paymentCreateDto.DurationServicesId);
            if (!durationCheck)
                throw new NotFoundException("Notfound");
            if (paymentCreateDto.Services == 0)
                throw new NotFoundException("Notfound");
            if (paymentCreateDto.Source == 0)
                throw new NotFoundException("Notfound");
            if (paymentCreateDto.ServiceType == 0)
                throw new NotFoundException("Notfound");
        }

        public async Task PaymentCreate(PaymentCreateDto paymentCreateDto)
        {
            var duration = await _unitOfWork.ServiceDurationRepository.GetAsync(x => x.Id == paymentCreateDto.DurationServicesId);

            var poster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == paymentCreateDto.PosterId);


            Payment payment = new Payment
            {
                Duration = duration.Duration,
                Amount = duration.Amount,
                AppUserId = paymentCreateDto.AppUserId,
                PosterId = paymentCreateDto.PosterId,
                Service = paymentCreateDto.Services,
                ServiceType = paymentCreateDto.ServiceType,
                Source = paymentCreateDto.Source,
            };
            await _unitOfWork.PaymentRepository.InsertAsync(payment);

            if (paymentCreateDto.ServiceType == ServiceType.Vip) poster.PosterFeatures.IsVip = true;
            else poster.PosterFeatures.IsPremium = true;

            await _unitOfWork.CommitAsync();
        }
    }
}
