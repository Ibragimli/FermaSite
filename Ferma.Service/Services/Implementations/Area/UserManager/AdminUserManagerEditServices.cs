using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.Area;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.Area.UserManagers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area.UserManagers
{
    public class AdminUserManagerEditServices : IAdminUserManagerEditServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public AdminUserManagerEditServices(UserManager<AppUser> UserManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = UserManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task EditUserManager(UserManagerEditDto userManagerEditDto)
        {
            bool checkBool = false;
            await DtoCheck(userManagerEditDto);

            var userExits = await GetUserManager(userManagerEditDto.Id);
            var newRole = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == userManagerEditDto.RoleId);

            if (!await _userManager.IsInRoleAsync(userExits, newRole.Name))
            {
                await _userManager.RemoveFromRoleAsync(userExits, userExits.RoleName);
                await _userManager.AddToRoleAsync(userExits, newRole.Name);
                checkBool = true;
            }

            if (userManagerEditDto.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(userExits);
                var errors = await _userManager.ResetPasswordAsync(userExits, token, userManagerEditDto.Password);
                if (!errors.Succeeded)
                {
                    foreach (var error in errors.Errors)
                    {
                        throw new UserPasswordResetException(error.Description);
                    }
                }
                checkBool = true;
            }
            if (userExits.RoleName != newRole.Name)
            {
                userExits.RoleName = newRole.Name;
                checkBool = true;
            }
            if (userExits.UserName != userManagerEditDto.Username)
            {
                userExits.UserName = userManagerEditDto.Username;
                userExits.NormalizedUserName = userManagerEditDto.Username.ToUpper();
                checkBool = true;
            }
            if (userExits.Name != userManagerEditDto.Name)
            {
                userExits.Name = userManagerEditDto.Name;
                checkBool = true;
            }
            if (userExits.IsAdmin != userManagerEditDto.IsAdmin)
            {
                userExits.IsAdmin = userManagerEditDto.IsAdmin;
                checkBool = true;
            }
            if (checkBool)
                await _unitOfWork.CommitAsync();
        }
        public async Task<AppUser> GetUserManager(string Id)
        {
            var UserExist = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == Id);

            return UserExist;
        }

      
        public async Task<string> RoleName(string id)
        {
            var roleName = await GetUserManager(id);
            return roleName.RoleName;
        }
        private async Task DtoCheck(UserManagerEditDto userManagerEditDto)
        {
            var UserExist = await _userManager.FindByNameAsync(userManagerEditDto.Username);

            if (UserExist != null && UserExist.Id != userManagerEditDto.Id)
                throw new ItemAlreadyException("Username database-də mövcüddur!");

        }
    }
}
