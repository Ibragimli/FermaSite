using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class ProfileEditServices : IProfileEditServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileEditServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CheckValue(ProfileEditDto editDto)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == editDto.UserId);

            if (editDto.UserId == null)
                throw new NotFoundException("user404");

            if (editDto.Email == null && editDto.Name == null)
                throw new ItemNullException("Email Null");

            if (user.Email == editDto.Email)
                throw new ValueAlreadyExpception("");
            if (user.Name == editDto.Name)
                throw new ValueAlreadyExpception("");
        }

        public async Task Edit(ProfileEditDto editDto)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == editDto.UserId);

            if (editDto.Email != null)
            {
                user.Email = editDto.Email;
                user.NormalizedEmail = editDto.Email.ToUpper();
            }
            
            if (editDto.Name != null)
                user.Name = editDto.Name;

            await _unitOfWork.CommitAsync();
        }
    }
}
