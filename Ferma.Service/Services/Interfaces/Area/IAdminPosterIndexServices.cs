using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.Area
{
    public interface IAdminPosterIndexServices
    {
        public IQueryable<Poster> GetPoster(string name, string phoneNumber, int subCategoryId);
        public Task<List<SubCategory>> GetSubCategories();
        public Task<List<Category>> GetCategories();

    }
}
