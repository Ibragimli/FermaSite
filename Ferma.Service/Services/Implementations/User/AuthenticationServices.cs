using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferma.Service.Services.Implementations.User
{
    public class AuthenticationServices : IAuthenticationServices
    {
        public string CreateToken()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
            var random = new Random();
            var token = new string(Enumerable.Repeat(chars, 40).Select(s => s[random.Next(s.Length)]).ToArray());
            return token;
        }
        public string CodeCreate()
        {
            Random random = new Random();
            string code = random.Next(100000, 999999).ToString();

            return code;
        }
    }
}
