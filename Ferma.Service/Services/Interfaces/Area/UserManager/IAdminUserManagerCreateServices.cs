using Ferma.Core.Entites;
using Ferma.Service.Dtos.Area;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.Area.UserManagers
{
    public interface IAdminUserManagerCreateServices
    {
        Task CreateUserManager(UserManagerCreateDto UserManagerCreateDto);
        Task<List<IdentityRole>> GetRoles();

    }
}
