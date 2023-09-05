using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.Area;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.Area.RoleManagers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area.RoleManagers
{
    public class AdminRoleManagerEditServices : IAdminRoleManagerEditServices
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public AdminRoleManagerEditServices(RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task EditRoleManager(RoleManagerEditDto roleManagerEditDto)
        {
            bool checkBool = false;

            var oldRoleManager = await GetRoleManager(roleManagerEditDto.Id);

            await DtoCheck(roleManagerEditDto);

            if (oldRoleManager.Name != roleManagerEditDto.RoleName)
            {
                oldRoleManager.Name = roleManagerEditDto.RoleName;
                oldRoleManager.NormalizedName = roleManagerEditDto.RoleName.ToUpper();
                checkBool = true;
            }

            if (checkBool)
                await _unitOfWork.CommitAsync();
        }

        public async Task<IdentityRole> GetRoleManager(string Id)
        {
            var roleExist = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == Id);

            return roleExist;
        }


        private async Task DtoCheck(RoleManagerEditDto roleManagerEditDto)
        {
            if (roleManagerEditDto == null)
                throw new ItemNullException("Role adını qeyd edin!");

            if (roleManagerEditDto.RoleName?.Length < 1)
                throw new ValueFormatExpception("Role adının uzunluğu minimum 1 ola bilər");

            var roleExist = _roleManager.FindByNameAsync(roleManagerEditDto.RoleName);

            if (roleExist.Result != null)
                throw new ItemUseException("Role database-də mövcüddur!");
        }
    }
}
