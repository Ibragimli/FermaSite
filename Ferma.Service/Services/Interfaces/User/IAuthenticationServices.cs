﻿using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IAuthenticationServices
    {
        public string CreateToken();
        public string CodeCreate();
        public string encryptSha256(string randomString);


        public Task<UserAuthentication> CreateAuthentication(string token, string code, string phoneNumber);

    }
}
