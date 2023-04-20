using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IProfileEditServices
    {
        public Task CheckValue(ProfileEditDto editDto);
        public Task Edit(ProfileEditDto editDto);
        public Task<PosterEditGetDto> EditVM(int id);
    }
}
