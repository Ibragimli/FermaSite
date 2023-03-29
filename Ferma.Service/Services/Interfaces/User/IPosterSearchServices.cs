using Ferma.Core.Entites;
using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IPosterSearchServices
    {
        public IQueryable<Poster> SearchPosterAll(SearchDto searchDto);
        public IQueryable<Poster> SearchPosterVip(SearchDto searchDto);
        public IQueryable<Poster> SearchPosterPremium(SearchDto searchDto);
    }
}
