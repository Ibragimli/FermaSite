using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class ProfileIndexServices : IProfileIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ProfileGetDto> _profileVM(AppUser user)
        {
            ProfileGetDto profileVM = new ProfileGetDto
            {
                User = user,
                ActivePosters = await _unitOfWork.PosterRepository.GetAllAsync(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Active && x.PosterFeatures.PhoneNumber == user.PhoneNumber, "PosterFeatures.City", "PosterFeatures", "PosterImages"),
                DeactivePosters = await _unitOfWork.PosterRepository.GetAllAsync(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.DeActive && x.PosterFeatures.PhoneNumber == user.PhoneNumber, "PosterFeatures.City", "PosterFeatures", "PosterImages"),
                WaitedPosters = await _unitOfWork.PosterRepository.GetAllAsync(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Waiting && x.PosterFeatures.PhoneNumber == user.PhoneNumber, "PosterFeatures.City", "PosterFeatures", "PosterImages"),
                DisabledPosters = await _unitOfWork.PosterRepository.GetAllAsync(x => !x.IsDelete && x.PosterFeatures.PosterStatus == PosterStatus.Disabled && x.PosterFeatures.PhoneNumber == user.PhoneNumber, "PosterFeatures.City", "PosterFeatures", "PosterImages"),
                PersonalPayments = await _unitOfWork.PaymentRepository.GetAllAsync(x => !x.IsDelete && x.Service == PaymentService.BalancePayment && x.AppUserId == user.Id),
                PosterPayments = await _unitOfWork.PaymentRepository.GetAllAsync(x => !x.IsDelete && x.Service == PaymentService.PosterPayment && x.AppUserId == user.Id, "AppUser", "Posters.PosterFeatures"),
                ProfileEditDto = new ProfileEditDto(),
            };
            return profileVM;
        }
    }
}
