using Ferma.Core.Entites;
using Ferma.Service.Dtos.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IPosterCreateServices
    {
        Task<PosterFeatures> CreatePosterFeature(PosterCreateDto PosterDto);
        void CreateImageString(List<string> imageFiles, int posterId);
        void CreateImageFormFile(List<IFormFile> imageFiles, int posterId);
        void CreatePosterCookie(List<IFormFile> imageFiles, PosterCreateDto posterCreateDto);
        void SendCode(string email, string code);
        Task<Poster> CreatePoster(PosterFeatures features);
        Task<Poster> CreatePosterForm(PosterFeatures features, List<IFormFile> imageFiles);
        void SaveChange(Poster Poster);
        void SaveContext(Poster Poster);

        Task<UserAuthentication> CheckAuthentication(string code, string phoneNumber, string token);
        string PhoneNumberFilter(string phoneNumber);
        PosterCreateDto GetPosterCookie();
        List<string> GetImageFilesCookie();
        Task<AppUser> CreateNewUser(string code, string phoneNumber, string email, string fullname);
        void CreatePosterUserId(string userId, int posterId, AppUser user);
        void ChangeAuthenticationStatus(UserAuthentication authentication);

    }
}
