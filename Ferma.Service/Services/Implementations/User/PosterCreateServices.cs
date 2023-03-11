using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Dtos.User;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{

    public class PosterCreateServices : IPosterCreateServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManageImageHelper _manageImageHelper;
        private readonly IEmailServices _emailServices;

        public PosterCreateServices(IUnitOfWork unitOfWork, IManageImageHelper manageImageHelper, IEmailServices emailServices) : base()
        {
            _unitOfWork = unitOfWork;
            _manageImageHelper = manageImageHelper;
            _emailServices = emailServices;
        }

        public async void CreateImage(List<IFormFile> imageFiles, int posterId)
        {
            int i = 1;
            bool posterStatus;
            foreach (var image in imageFiles)
            {
                posterStatus = false;
                if (i == 1)
                    posterStatus = true;
                PosterImage Posterimage = new PosterImage
                {
                    IsPoster = posterStatus,
                    PosterId = posterId,
                    Image = _manageImageHelper.FileSave(image, "poster"),
                };
                await _unitOfWork.PosterImageRepository.InsertAsync(Posterimage);
                i++;
            }
            await _unitOfWork.CommitAsync();
        }
        public async Task<PosterFeatures> CreatePosterFeature(PosterCreateDto PosterDto)
        {
            PosterFeatures features = new PosterFeatures
            {
                Name = PosterDto.PosterName,
                CityId = PosterDto.CityId,
                Describe = PosterDto.Describe,
                Email = PosterDto.Email,
                PhoneNumber = PosterDto.PhoneNumber,
                SubCategoryId = PosterDto.SubCategoryId,
                Price = PosterDto.Price,
                PriceCurrency = PosterDto.PriceCurrency,
                ViewCount = 0,
                WishCount = 0,
                IsPremium = false,
                IsVip = false,
                PosterStatus = PosterStatus.Waiting,
                IsDisabled = false,
                ModifiedDate = DateTime.UtcNow.AddHours(4),
                IsDelete = false,

            };
            await _unitOfWork.PosterFeaturesRepository.InsertAsync(features);
            await _unitOfWork.CommitAsync();
            return features;
        }



        public async Task<Poster> CreatePoster(PosterFeatures features, List<IFormFile> imageFiles)
        {
            Poster poster = new Poster
            {
                PosterFeaturesId = features.Id,
                ImageFiles = imageFiles,
            };
            await _unitOfWork.PosterRepository.InsertAsync(poster);
            await _unitOfWork.CommitAsync();
            return poster;
        }




        public async void SaveChange(Poster Poster)
        {
            await _unitOfWork.PosterRepository.InsertAsync(Poster);
        }
        public async void SaveContext(Poster Poster)
        {
            await _unitOfWork.CommitAsync();
        }


        public string AutenticationCodeCreate()
        {
            Random random = new Random();
            string code = random.Next(100000, 999999).ToString();

            return code;
        }

        public void SendCode(string email, string code)
        {
            _emailServices.Send(email, "Doğrulama kodunuz", code);
        }

        public string CreateUrl(string email)
        {
            throw new NotImplementedException();
        }

        //public string CreateUrl(string email)
        //{
        //    string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        //    var random = new Random();
        //    var token = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        //    var url = _urlHelper.Action("NumberAuthentication", "elanlar", new { email = email, token = token }, _httpContextAccessor.HttpContext.Request.Scheme);
        //    return url;
        //}
    }
}
