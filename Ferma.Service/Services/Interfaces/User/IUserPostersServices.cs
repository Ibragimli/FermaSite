using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IUserPostersServices
    {
        public IQueryable<Poster> VipPosters(string phoneNumber);
        public IQueryable<Poster> AllPosters(string phoneNumber);
        public IQueryable<Poster> PremiumPosters(string phoneNumber);
        
    }
}
