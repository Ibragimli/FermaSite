using Ferma.Core.Entites;
using Ferma.Service.Dtos.Area;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.Area
{
    public interface IAdminPosterEditServices
    {
        public void CheckPostEdit(Poster poster);
        public Task EditPoster(Poster poster);
        public Task PosterActive(int id);
        public Task PosterDisabled(int id);

    }
}
