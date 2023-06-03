using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
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

        public string encryptSha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        public string CodeCreate()
        {
            Random random = new Random();
            string code = random.Next(100000, 999999).ToString();

            return code;
        }
        public async Task<UserAuthentication> CreateAuthentication(string token, string code, string phoneNumber)
        {
            //RareLimitCheck
            await RareLimit(phoneNumber);

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
        private async Task RareLimit(string phoneNumber)
        {
            var before = DateTime.UtcNow.AddHours(4).Minute - 5;
            var now = DateTime.UtcNow.AddHours(4);
            var count = await _unitOfWork.UserAuthenticationRepository.GetTotalCountAsync(x => x.CreatedDate.Minute > before && x.CreatedDate < now && x.PhoneNumber == phoneNumber);
            if (count > 5)
                throw new RareLimitException("5 dəqiqə ərzindəki sorğu limitini keçmisiz! Biraz gözləməyiniz xahiş olunur.");
        }
    }
}
