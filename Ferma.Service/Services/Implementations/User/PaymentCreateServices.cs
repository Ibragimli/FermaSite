using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class PaymentCreateServices : IPaymentCreateServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentCreateServices(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task PaymentCheck(PaymentCreateDto paymentCreateDto)
        {

            bool posterCheck = await _unitOfWork.PosterRepository.IsExistAsync(x => x.Id == paymentCreateDto.PosterId);
            if (!posterCheck)
                throw new NotFoundException("Notfound");
            if (paymentCreateDto.PosterStatus != PosterStatus.Active)
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
        public async Task PaymentCreateBalance(PaymentCreateDto paymentCreateDto)
        {
            if (paymentCreateDto.Source != Source.Balance)
                throw new NotFoundException("Notfound");
            if (paymentCreateDto.AppUserId == null)
                throw new NotFoundException("Notfound");
            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == paymentCreateDto.AppUserId);
            if (user == null)
                throw new NotFoundException("Notfound");

            var duration = await _unitOfWork.ServiceDurationRepository.GetAsync(x => x.Id == paymentCreateDto.DurationServicesId);

            if (user.Balance < duration.Amount)
            {
                throw new PaymentValueException("Balansınızda kifayət qədər məbləğ yoxdur!");
            }
            var poster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == paymentCreateDto.PosterId, "PosterFeatures");

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
            if (paymentCreateDto.ServiceType == ServiceType.Vip)
            {
                poster.PosterFeatures.IsVip = true;
                poster.PosterFeatures.ExpirationDateVip = DateTime.Now.AddDays(duration.Duration);
            }
            else
            {
                poster.PosterFeatures.IsPremium = true;
                poster.PosterFeatures.ExpirationDatePremium = DateTime.Now.AddDays(duration.Duration);
            }
            user.Balance = user.Balance - duration.Amount;
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("paymentDto");
            await _unitOfWork.CommitAsync();
        }
        public async Task PaymentCreateBankCard(PaymentCreateDto paymentCreateDto)
        {
            var duration = await _unitOfWork.ServiceDurationRepository.GetAsync(x => x.Id == paymentCreateDto.DurationServicesId);

            var poster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == paymentCreateDto.PosterId, "PosterFeatures");
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
            if (paymentCreateDto.ServiceType == ServiceType.Vip)
            {
                poster.PosterFeatures.IsVip = true;
                poster.PosterFeatures.ExpirationDateVip = DateTime.Now.AddDays(duration.Duration);
            }
            else
            {
                poster.PosterFeatures.IsPremium = true;
                poster.PosterFeatures.ExpirationDatePremium = DateTime.Now.AddDays(duration.Duration);
            }
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("paymentDto");

            await _unitOfWork.CommitAsync();
        }
    }
}
