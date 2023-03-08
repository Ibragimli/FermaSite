using Ferma.Core.Entites;
using Ferma.Core.Repositories;
using Ferma.Data.Datacontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferma.Data.Repositories
{

    public class PosterRepository : Repository<Poster>, IPosterRepository
    {
        private readonly DataContext _context;

        public PosterRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Poster> asQueryableProduct()
        {
            var posters = _context.Posters
                .Include(x => x.PosterImages)
                .Include(x => x.PosterFeatures)
                .AsQueryable();
            return posters;
        }
    }

}
