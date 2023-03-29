using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IAnaSehifeIndexServices
    {
        public IQueryable<Poster> GetPostersAsync();
        public IQueryable<Poster> GetVipPosterAsync();
        public IQueryable<Poster> GetPremiumPosterAsync();
        public IQueryable<Poster> GetPrePosterAsync();
        
        public Task<List<Category>> GetAllCategoryAsync();

    }
}
