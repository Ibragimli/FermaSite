using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{

    public class PosterCreateServices : IPosterCreateServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManageImageHelper _manageImageHelper;
        private readonly IEmailServices _emailServices;
        private readonly IHttpContextAccessor _contextAccessor;

        public PosterCreateServices(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IManageImageHelper manageImageHelper, IEmailServices emailServices, IHttpContextAccessor contextAccessor) : base()
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _manageImageHelper = manageImageHelper;
            _emailServices = emailServices;
            _contextAccessor = contextAccessor;
        }
        public async Task CreateImageFormFile(List<IFormFile> imageFiles, int posterId)
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

        public async Task CreateImageString(List<string> imageFiles, int posterId)
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
                    Image = image,
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



        public async Task<Poster> CreatePoster(PosterFeatures features)
        {
            Poster poster = new Poster
            {
                PosterFeaturesId = features.Id,
            };
            await _unitOfWork.PosterRepository.InsertAsync(poster);
            await _unitOfWork.CommitAsync();
            return poster;
        }

        public async Task<Poster> CreatePosterForm(PosterFeatures features, List<IFormFile> imageFiles)
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




        public void SendCode(string email, string code)
        {
            _emailServices.Send(email, "Doğrulama kodunuz", code);
        }

        public string CreateUrl(string email)
        {
            throw new NotImplementedException();
        }

        public void CreatePosterCookie(List<IFormFile> imageFiles, PosterCreateDto posterCreateDto)
        {
            foreach (var item in imageFiles)
            {
                var filename = _manageImageHelper.FileSave(item, "poster");
                posterCreateDto.ImageFilesStr.Add(filename);
            }
            var posterImageStr = JsonConvert.SerializeObject(posterCreateDto.ImageFilesStr);
            _contextAccessor.HttpContext.Response.Cookies.Append("posterImageFiles", posterImageStr);
            posterCreateDto.ImageFiles = null;
            var posterStr = JsonConvert.SerializeObject(posterCreateDto);
            _contextAccessor.HttpContext.Response.Cookies.Append("posterVM", posterStr);
        }



        public string PhoneNumberFilter(string phoneNumber)
        {
            string number = "";
            string[] charsNumber = phoneNumber.Split('_', '-', ' ', '(', ')', ',', '.', '/', '?', '!', '+', '=', '|', '.');

            foreach (var item in charsNumber)
            {
                number += item;
            }
            return number;
        }
        public async Task<UserAuthentication> CheckAuthentication(string code, string phoneNumber, string token)
        {
            var authentication = await _unitOfWork.UserAuthenticationRepository.GetAsync(x => x.IsDisabled == false && x.Code == code && x.Token == token && x.PhoneNumber == phoneNumber);
            var existAuthentication = await _unitOfWork.UserAuthenticationRepository.GetAsync(x => x.IsDisabled == false && x.Token == token);
            //Kod yanlişdir erroru və təkrar yoxlama limiti
            if (authentication == null)
            {
                if (existAuthentication != null)
                {
                    if (existAuthentication.Count > 1)
                        existAuthentication.Count -= 1;
                    else
                    {
                        existAuthentication.IsDisabled = true;
                    }
                    await _unitOfWork.CommitAsync();
                }
                throw new AuthenticationCodeException("Kod yanlışdır!");
            }
            return authentication;
        }

        public PosterCreateDto GetPosterCookie()
        {
            PosterCreateDto posterCreateDto = new PosterCreateDto();

            //cookie
            string posterItem = _contextAccessor.HttpContext.Request.Cookies["posterVM"];

            if (posterItem != null)
            {
                posterCreateDto = JsonConvert.DeserializeObject<PosterCreateDto>(posterItem);
            }
            else
            {
                throw new CookieNotActiveException("Cookie-nizi aktiv edin!");
            }
            return posterCreateDto;

        }

        public List<string> GetImageFilesCookie()
        {
            List<string> images = new List<string>();
            string imageItem = _contextAccessor.HttpContext.Request.Cookies["posterImageFiles"];

            if (imageItem != null)
            {
                images = JsonConvert.DeserializeObject<List<string>>(imageItem);
            }
            else
            {
                throw new CookieNotActiveException("Cookie-nizi aktiv edin!");
            }
            return images;
        }

        public async Task<AppUser> CreateNewUser(string code, string phoneNumber, string email, string fullname)
        {
            AppUser newUser = new AppUser();
            //hesab yaradmaq

            var UserExists = await _unitOfWork.UserRepository.GetAsync(x => x.PhoneNumber == phoneNumber);
            if (UserExists == null)
            {
                newUser = new AppUser
                {
                    UserName = phoneNumber,
                    PhoneNumber = phoneNumber,
                    IsAdmin = false,
                    Balance = 0,
                    Email = email,
                    Name = fullname,
                };
                var result = await _userManager.CreateAsync(newUser, code);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        throw new Exception(error.Description);
                    }
                }
                await _userManager.AddToRoleAsync(newUser, "User");
                await _unitOfWork.CommitAsync();
                return newUser;

            }
            return UserExists;
            //hesab yaradmaq
        }



        public async Task CreatePosterUserId(string userId, int posterId, AppUser user)
        {
            PosterUserId posterUserId = new PosterUserId();

            //poster user elaqesi
            posterUserId = new PosterUserId
            {
                AppUserId = user.Id,
                PosterId = posterId,
            };
            await _unitOfWork.PosterUserIdRepository.InsertAsync(posterUserId);
            await _unitOfWork.CommitAsync();
        }
        public async Task ChangeAuthenticationStatus(UserAuthentication authentication)
        {
            authentication.IsDisabled = true;
            await _unitOfWork.CommitAsync();
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
