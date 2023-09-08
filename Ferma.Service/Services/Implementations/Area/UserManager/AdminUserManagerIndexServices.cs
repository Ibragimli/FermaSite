using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
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
    public class AdminUserManagerIndexServices : IAdminUserManagerIndexServices
    {
        private readonly UserManager<AppUser> _UserManager;

        public AdminUserManagerIndexServices(UserManager<AppUser> UserManager)
        {
            _UserManager = UserManager;
        }
        public IQueryable<AppUser> GetUserManager(string name)
        {
            var Users = _UserManager.Users.Where(x => x.UserName != null).AsQueryable();


            if (Users != null)
            {
                Users = Users.Where(i => EF.Functions.Like(i.UserName, $"%{name}%"));
                Users = Users.Where(i => !i.IsAdmin);
            }
            return Users;
        }
        public IQueryable<AppUser> GetAdminManager(string name)
        {
            var Users = _UserManager.Users.Where(x => x.UserName != null).AsQueryable();


            if (Users != null)
            {
                Users = Users.Where(i => EF.Functions.Like(i.UserName, $"%{name}%"));
                Users = Users.Where(i => i.IsAdmin);

            }

            return Users;
        }
    }
}
