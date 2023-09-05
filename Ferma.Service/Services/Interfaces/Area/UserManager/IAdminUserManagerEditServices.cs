using Ferma.Core.Entites;
using Ferma.Service.Dtos.Area;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.Area.UserManagers
{
    public interface IAdminUserManagerEditServices
    {
        public Task<AppUser> GetUserManager(string Id);
        public Task EditUserManager(UserManagerEditDto UserManagerEditDto);
        public Task<string> RoleName(string id);

    }
}
