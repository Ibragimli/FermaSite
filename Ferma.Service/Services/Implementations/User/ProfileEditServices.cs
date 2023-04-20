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

        }

        public async Task Edit(ProfileEditDto editDto)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == editDto.UserId);
            bool checkBool = false;
            if (user.Email != editDto.Email)
                if (editDto.Email != null)
                {
                    user.Email = editDto.Email;
                    user.NormalizedEmail = editDto.Email.ToUpper();
                    checkBool = true;
                }

            if (user.Name != editDto.Name)
                if (editDto.Name != null)
                {
                    user.Name = editDto.Name;
                    checkBool = true;
                }
            if (checkBool)
                await _unitOfWork.CommitAsync();
        }

        public async Task<PosterEditGetDto> EditVM(int id)
        {
            PosterEditGetDto posterEditVM = new PosterEditGetDto
            {
                PosterEditDto = new PosterEditDto(),

                Poster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == id && x.IsDelete == false && x.PosterFeatures.IsDisabled == false,
                "PosterImages", "PosterFeatures.SubCategory", "PosterFeatures.SubCategory.Category", "PosterUserIds.AppUser", "PosterFeatures.City"),

                Categories = await _unitOfWork.CategoryRepository.GetAllAsync(x => !x.IsDelete),
                SubCategories = await _unitOfWork.SubCategoryRepository.GetAllAsync(x => !x.IsDelete),
            };
            return posterEditVM;
        }
    }
}
