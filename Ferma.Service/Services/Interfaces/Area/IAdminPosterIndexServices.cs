using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferma.Service.Services.Interfaces.Area
{
    public interface IAdminPosterIndexServices
    {
        public IQueryable<Poster> GetPoster();
    }
}
