using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class SettingIndexServices : ISettingIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;


        public SettingIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IQueryable<Setting> SearchCheck(string search)
        {
            var SettingLast = _unitOfWork.SettingRepository.asQueryable();
            if (search != null)
            {
                search = search.ToLower();
                if (search != null)
                    SettingLast = SettingLast.Where(i => EF.Functions.Like(i.Key, $"%{search}%"));
            }
            return SettingLast;
        }

    }
}
