using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string CreateToken()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
            var random = new Random();
            var token = new string(Enumerable.Repeat(chars, 30).Select(s => s[random.Next(s.Length)]).ToArray());
            return token;
        }

        public string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public string CodeCreate()
        {
            Random random = new Random();
            string code = random.Next(100000, 999999).ToString();

            return code;
        }
        public async Task<UserAuthentication> CreateAuthentication(string token, string code, string phoneNumber)
        {
            var oldAuthentication = await _unitOfWork.UserAuthenticationRepository.GetAllAsync(x => x.IsDisabled == false && x.PhoneNumber == phoneNumber);

            if (oldAuthentication != null)
            {
                foreach (var item in oldAuthentication)
                {
                    item.IsDisabled = true;
                }
            }

            UserAuthentication authentication = new UserAuthentication
            {
                Code = code,
                Token = token,
                IsDisabled = false,
                PhoneNumber = phoneNumber,
                Count = 3,
            };
            await _unitOfWork.UserAuthenticationRepository.InsertAsync(authentication);
            await _unitOfWork.CommitAsync();
            return authentication;
        }
    }
}
