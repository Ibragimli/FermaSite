using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{

    public class PosterCreateIndexServices : IPosterCreateIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PosterCreateIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Category>> Categories()
        {
            throw new NotImplementedException();

        }

        public Task<List<City>> Cities()
        {
            throw new NotImplementedException();
        }

        //public async Task<List<City>> Cities()
        //{
        //    return await _unitOfWork.CityRepository.GetAllAsync(x => !x.IsDelete);

        //}

        public PosterCreateDto PosterCreateDto()
        {
            throw new NotImplementedException();
        }

        public Task<List<SubCategory>> SubCategories()
        {
            throw new NotImplementedException();
        }
    }
}
