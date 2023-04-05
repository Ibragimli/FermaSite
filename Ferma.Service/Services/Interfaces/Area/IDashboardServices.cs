using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.Area
{
    public interface IDashboardServices
    {
        public Task<int> AllPosterCount();
        public Task<int> VipPosterCount();
        public Task<int> PremiumPosterCount();
        public Task<int> NewPosterCount();
        public Task<int> NewContactCount();
        public Task<decimal?> PaymentMoney();
        public Task<List<Payment>> Payments();
    }
}
