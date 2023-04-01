﻿using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.Area
{
    public interface IAdminCityServices
    {
        public IQueryable<City> GetCities(string name);
        public Task<City> GetCity(int id);
        public Task CityCreate(City City);
        public Task CityEdit(City City);
        public Task CityDelete(int id);
    }
}
