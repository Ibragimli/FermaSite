using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IPaymentCreateServices
    {
        public Task PaymentCheck(PaymentCreateDto paymentCreateDto);
        public Task PaymentCreateBankCard(PaymentCreateDto paymentCreateDto);
        public Task PaymentCreateBalance(PaymentCreateDto paymentCreateDto);
    }
}
