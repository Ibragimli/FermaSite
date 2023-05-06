using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface ISmsSenderServices
    {
        public Task<bool> SmsSend(string number, string code);

    }
}
