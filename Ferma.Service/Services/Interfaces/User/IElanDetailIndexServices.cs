using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IElanDetailIndexServices
    {
        public Task<Poster> GetPoster(int id);
        public Task<List<Poster>> GetSimilarPosters(int id);
    }
}
