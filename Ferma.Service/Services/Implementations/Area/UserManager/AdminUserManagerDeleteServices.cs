using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Data.Datacontext;
using Ferma.Service.CustomExceptions;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.Area.UserManagers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area.UserManagers
{
    public class AdminUserManagerDeleteServices : IAdminUserManagerDeleteServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public AdminUserManagerDeleteServices(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task DeleteUserManager(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                throw new ItemNullException("User tapılmadı!");

            await _userManager.DeleteAsync(user);
            await _unitOfWork.CommitAsync();

        }
    }
}
