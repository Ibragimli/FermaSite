using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IBalanceIncreaseServices
    {
        public Task CheckBalanceIncrease(BalanceDto balanceDto);
        public Task BalanceIncrease(BalanceDto balanceDto);

    }
}
