using Ferma.Service.Dtos.Area;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.Area
{
    public interface IAdminLoginServices
    {
        Task<bool> Login(AdminLoginPostDto adminLoginPostDto);
        void Logout();
    }
}
