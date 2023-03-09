using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var list = await _unitOfWork.CategoryRepository.GetAllAsync(x => !x.IsDelete);

            return list.ToList(); 
        }

        public async Task<List<City>> Cities()
        {
            var list = await _unitOfWork.CityRepository.GetAllAsync(x => !x.IsDelete);
            return list.ToList(); ;

        }

        public PosterCreateDto PosterDto()
        {
            PosterCreateDto posterCreateDto = new PosterCreateDto();
            return posterCreateDto;
        }

        public async Task<List<SubCategory>> SubCategories()
        {
            var list = await _unitOfWork.SubCategoryRepository.GetAllAsync(x => !x.IsDelete);
            return list.ToList(); ;
        }
    }
}
