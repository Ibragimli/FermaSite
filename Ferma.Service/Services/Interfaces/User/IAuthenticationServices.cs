using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IAuthenticationServices
    {
        public string CreateToken();
        public string CodeCreate();
    }
}
