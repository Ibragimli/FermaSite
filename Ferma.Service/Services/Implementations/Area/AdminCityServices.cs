using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class AdminCityServices : IAdminCityServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminCityServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CityCreate(City City)
        {
            City newCity = new City();
            bool check = false;
            if (City.Name != null)
            {
                if (City.Name.Length > 30)
                    throw new ItemFormatException("Şəhərin adı maksimum  uzunluğu 30 ola bilər");
                newCity.Name = City.Name;
                check = true;
            }
            else
                throw new ItemNullException("Şəhərin adı boş ola bilməz");

            if (check)
            {
                await _unitOfWork.CityRepository.InsertAsync(newCity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task CityEdit(City City)
        {
            bool check = false;
            var oldCity = await _unitOfWork.CityRepository.GetAsync(x => x.Id == City.Id);

            if (City.Name != null)
            {
                if (City.Name.Length > 30)
                    throw new ItemFormatException("Şəhərin adının maksimum  uzunluğu 30 ola bilər");
                if (City.Name != oldCity.Name)
                {
                    oldCity.Name = City.Name;
                    check = true;
                }
            }
            else
                throw new ItemNullException("Kategoriya boş ola bilməz");

            if (check)
            {
                oldCity.ModifiedDate = DateTime.UtcNow.AddHours(4);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task CityDelete(int id)
        {
            var oldCity = await _unitOfWork.CityRepository.GetAsync(x => x.Id == id);
            bool check = await _unitOfWork.PosterFeaturesRepository.IsExistAsync(x => x.CityId == id);
            if (check)
                throw new ItemUseException("Şəhər elanda istifadə olunur!!!");

            _unitOfWork.CityRepository.Remove(oldCity);
            await _unitOfWork.CommitAsync();
        }



        public async Task<City> GetCity(int id)
        {
            var City = await _unitOfWork.CityRepository.GetAsync(x => x.Id == id && !x.IsDelete);

            return City;
        }
        public IQueryable<City> GetCities(string name)
        {
            var City = _unitOfWork.CityRepository.asQueryable();
            City = City.Where(x => !x.IsDelete);
            if (name != null)
                City = City.Where(i => EF.Functions.Like(i.Name, $"%{name}%"));

            return City;
        }
    }
}
