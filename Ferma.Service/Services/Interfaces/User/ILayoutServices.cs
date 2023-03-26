using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface ILayoutServices
    {
        public Task<List<Setting>> GetSettingsAsync();
        public Task<List<City>> GetAllCitiesSearch();

    }
}
