using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.Area;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class PosterDeleteServices : IPosterDeleteServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManageImageHelper _manageImageHelper;

        public PosterDeleteServices(IUnitOfWork unitOfWork, IManageImageHelper manageImageHelper)
        {
            _unitOfWork = unitOfWork;
            _manageImageHelper = manageImageHelper;
        }
        public async Task DeletePoster(int id)
        {
            bool check = false;
            var poster = await _unitOfWork.PosterRepository.GetAsync(x => !x.IsDelete && x.Id == id);
            if (poster == null)
                throw new ItemNotFoundException("404");
            var images = await _unitOfWork.PosterImageRepository.GetAllAsync(x => x.PosterId == poster.Id && !x.IsDelete);
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync(x => !x.IsDelete && x.PosterId == poster.Id);
            var posterUserIds = await _unitOfWork.PosterUserIdRepository.GetAllAsync(x => !x.IsDelete && x.PosterId == poster.Id);
            var wishItems = await _unitOfWork.WishItemRepository.GetAllAsync(x => !x.IsDelete && x.PosterId == poster.Id);
            var feature = await _unitOfWork.PosterFeaturesRepository.GetAsync(x => x.Id == poster.PosterFeaturesId && !x.IsDelete);

            if (images != null)
            {
                foreach (var image in images)
                {
                    _unitOfWork.PosterImageRepository.Remove(image);
                    _manageImageHelper.DeleteFile(image.Image, "poster");
                }
                check = true;
            }
            if (payments != null)
            {
                foreach (var payment in payments)
                {
                    _unitOfWork.PaymentRepository.Remove(payment);
                }
                check = true;

            }
            if (posterUserIds != null)
            {
                foreach (var posterUserId in posterUserIds)
                {
                    _unitOfWork.PosterUserIdRepository.Remove(posterUserId);
                }
                check = true;
            }
            if (wishItems != null)
            {
                foreach (var wishItem in wishItems)
                {
                    _unitOfWork.WishItemRepository.Remove(wishItem);
                }
                check = true;
            }
            if (check)
            {
                _unitOfWork.PosterRepository.Remove(poster);
                _unitOfWork.PosterFeaturesRepository.Remove(feature);
                await _unitOfWork.CommitAsync();
            }

        }
    }
}
