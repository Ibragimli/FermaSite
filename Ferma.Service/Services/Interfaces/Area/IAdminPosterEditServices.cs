using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.Area
{
    public interface IAdminPosterEditServices
    {
        public Task<Poster> GetPoster(int id);
        public Task<List<SubCategory>> GetSubCategories();
        public Task<List<Category>> GetCategories();
        public Task<List<City>> GetAllCity();
        public Task<PosterUserId> GetAppUser(int posterId);

    }
}
