using Ferma.Core.Entites;
using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IPosterCreateIndexServices
    {
        public PosterCreateDto PosterCreateDto();
        public Task<List<City>> Cities();
        public Task<List<Category>> Categories();
        public Task<List<SubCategory>> SubCategories();

    }
}
