using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IPosterEditServices
    {
        public Task posterDisabled(int id);
        public Task posterEdit(Poster poster );
    }
}
