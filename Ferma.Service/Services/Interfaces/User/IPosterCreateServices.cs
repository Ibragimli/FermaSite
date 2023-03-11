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
        void CreateImage(List<IFormFile> imageFiles, int posterId);
        void SendCode(string email, string code);
        Task<Poster> CreatePoster(PosterFeatures features, List<IFormFile> imageFiles);
        void SaveChange(Poster Poster);
        void SaveContext(Poster Poster);
        Task<PosterFeatures> CreatePosterFeature(PosterCreateDto PosterDto);
        string AutenticationCodeCreate();
        string CreateUrl(string email);
    }
}
