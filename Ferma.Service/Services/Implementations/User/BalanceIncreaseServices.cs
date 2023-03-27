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
    public class BalanceIncreaseServices : IBalanceIncreaseServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public BalanceIncreaseServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task BalanceIncrease(BalanceDto balanceDto)
        {
            AppUser user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == balanceDto.AppUserId);
            user.Balance += balanceDto.Balance;

            Payment payment = new Payment
            {
                Amount = balanceDto.Balance,
                AppUserId = balanceDto.AppUserId,
                Service = PaymentService.BalancePayment,
                Source = Source.BankCard,
            };
            await _unitOfWork.PaymentRepository.InsertAsync(payment);
            await _unitOfWork.CommitAsync();
        }

        public async Task CheckBalanceIncrease(BalanceDto balanceDto)
        {
            if (balanceDto.AppUserId == null)
                throw new UserNotFoundException("");
            AppUser user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == balanceDto.AppUserId);
            if (user == null)
                throw new UserNotFoundException("");
            if (balanceDto.Source != Source.BankCard)
                balanceDto.Source = Source.BankCard;
            if (balanceDto.Balance < 1 || balanceDto.Balance > 1000)
                throw new PaymentValueException("Balansınızı 0 və 1000 AZN aralığında artıra bilərsinz!");

        }
    }
}
