using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Services.Interfaces.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferma.Service.Services.Implementations.Area
{
    public class BalancePaymentIndexServices : IBalancePaymentIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public BalancePaymentIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<Payment> GetPayments(int year, int month)
        {
            var payments = _unitOfWork.PaymentRepository.asQueryable("AppUser", "Posters.PosterFeatures");
            payments = payments.Where(x => !x.IsDelete);
            payments = payments.Where(x => x.Service == PaymentService.BalancePayment);
            if (year != 0)
                payments = payments.Where(x => x.CreatedDate.Year == year);
            if (month != 0)
                payments = payments.Where(x => x.CreatedDate.Month == month);

            return payments;
        }
    }
}
