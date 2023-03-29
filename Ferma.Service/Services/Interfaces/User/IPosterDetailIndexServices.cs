using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IPosterDetailIndexServices
    {
        public Task<Poster> GetPoster(int id);
        public Task PosterViewCount(Poster poster);
        public Task<PosterUserId> GetUser(int id);
        public Task<List<Poster>> GetSimilarPoster(int id, Poster poster);
        public Task<List<ServiceDuration>> GetServiceDurations();
        public Task<int> GetWishCount(int id);


    }
}
