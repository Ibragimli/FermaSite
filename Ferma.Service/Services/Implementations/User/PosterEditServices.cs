using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class PosterEditServices : IPosterEditServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManageImageHelper _manageImageHelper;

        public PosterEditServices(IUnitOfWork unitOfWork, IManageImageHelper manageImageHelper)
        {
            _unitOfWork = unitOfWork;
            _manageImageHelper = manageImageHelper;
        }
        public async Task posterDisabled(int id)
        {
            Poster poster = new Poster();
            if (id != 0)
                poster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == id);

            if (poster == null)
                throw new ItemNotFoundException("Elan tapılmadı");
            poster.PosterFeatures.PosterStatus = PosterStatus.DeActive;
            poster.PosterFeatures.IsPremium = false;
            poster.PosterFeatures.IsVip = false;
            await _unitOfWork.CommitAsync();
        }






        public async Task posterEdit(Poster poster)
        {
            bool checkBool = false;
            if (poster == null)
                throw new ItemNotFoundException("Elan tapılmadı");

            if (poster.Id == 0)
                throw new ItemNotFoundException("Elan tapılmadı");
            var oldPoster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == poster.Id, "PosterFeatures", "PosterImages");

            if (poster.PosterImageFile != null)
                _manageImageHelper.PosterCheck(poster.PosterImageFile);
            if (poster.ImageFiles != null)
                _manageImageHelper.ImagesCheck(poster.ImageFiles);

         
            if (poster.PosterFeatures.SubCategoryId != 0 && oldPoster.PosterFeatures.SubCategoryId != poster.PosterFeatures.SubCategoryId)
            {
                oldPoster.PosterFeatures.SubCategoryId = poster.PosterFeatures.SubCategoryId;
                checkBool = true;
            }
            if (poster.PosterFeatures.Name != null && oldPoster.PosterFeatures.Name != poster.PosterFeatures.Name)
            {
                oldPoster.PosterFeatures.Name = poster.PosterFeatures.Name;
                checkBool = true;
            }

            if (poster.PosterFeatures.Describe != null && oldPoster.PosterFeatures.Describe != poster.PosterFeatures.Describe)
            {
                oldPoster.PosterFeatures.Describe = poster.PosterFeatures.Describe;
                checkBool = true;
            }

            if (poster.PosterFeatures.Price != 0 && oldPoster.PosterFeatures.Price != poster.PosterFeatures.Price)
            {
                oldPoster.PosterFeatures.Price = poster.PosterFeatures.Price;
                checkBool = true;
            }
            if (oldPoster.PosterFeatures.PriceCurrency != poster.PosterFeatures.PriceCurrency)
            {
                oldPoster.PosterFeatures.PriceCurrency = poster.PosterFeatures.PriceCurrency;
                checkBool = true;
            }
            if (oldPoster.PosterFeatures.IsShipping != poster.PosterFeatures.IsShipping)
            {
                oldPoster.PosterFeatures.IsShipping = poster.PosterFeatures.IsShipping;
                checkBool = true;
            }
            if (oldPoster.PosterFeatures.IsNew != poster.PosterFeatures.IsNew)
            {
                oldPoster.PosterFeatures.IsNew = poster.PosterFeatures.IsNew;
                checkBool = true;
            }

            DeleteImages(poster, oldPoster);
            await CreateImageFormFile(poster.ImageFiles, poster.Id);
            PosterImageChange(poster, oldPoster);

            if (checkBool)
                await _unitOfWork.CommitAsync();


        }
        private void PosterImageChange(Poster poster, Poster posterExist)
        {

            if (poster.PosterImageFile != null)
            {
                var posterImageFile = poster.PosterImageFile;

                PosterImage posterImage = posterExist.PosterImages.FirstOrDefault(x => x.IsPoster);

                if (posterImage == null) throw new ImageNullException("Şəkil tapılmadı!");

                string filename = _manageImageHelper.FileSave(posterImageFile, "poster");
                _manageImageHelper.DeleteFile(posterImage.Image, "poster");
                posterImage.Image = filename;
                posterImage.IsPoster = true;
            }
        }

        private void DeleteImages(Poster poster, Poster posterExist)
        {

            var posterImages = posterExist.PosterImages;
            if (poster.PosterImagesIds != null)
            {
                foreach (var item in posterImages.ToList().Where(x => x.IsDelete == false && !x.IsPoster && !poster.PosterImagesIds.Contains(x.Id)))
                {
                    _manageImageHelper.DeleteFile(item.Image, "poster");
                    posterExist.PosterImages.Remove(item);
                }
                posterImages.ToList().RemoveAll(x => !poster.PosterImagesIds.Contains(x.Id));

            }
            else
            {
                if (poster.ImageFiles?.Count() > 0)
                {
                    foreach (var item in posterImages.ToList().Where(x => !x.IsDelete && !x.IsPoster))
                    {
                        _manageImageHelper.DeleteFile(item.Image, "poster");
                        posterExist.PosterImages.Remove(item);
                    }
                }
                else throw new ImageCountException("Axırıncı şəkil silinə bilməz!");
            }
        }
        private async Task CreateImageFormFile(List<IFormFile> imageFiles, int posterId)
        {
            if (imageFiles != null)
                foreach (var image in imageFiles)
                {

                    PosterImage Posterimage = new PosterImage
                    {
                        IsPoster = false,
                        PosterId = posterId,
                        Image = _manageImageHelper.FileSave(image, "poster"),
                    };
                    await _unitOfWork.PosterImageRepository.InsertAsync(Posterimage);
                }
        }

    }
}
