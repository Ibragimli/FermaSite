using Ferma.Core.Entites;
using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IProfileIndexServices
    {
        Task<ProfileGetDto> _profileVM(AppUser user);

    }
}
