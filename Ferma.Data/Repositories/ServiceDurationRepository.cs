using Ferma.Core.Entites;
using Ferma.Core.Repositories;
using Ferma.Data.Datacontext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Data.Repositories
{
    public class ServiceDurationRepository : Repository<ServiceDuration>, IServiceDurationRepository
    {
        private readonly DataContext _context;

        public ServiceDurationRepository(DataContext context) : base(context)
        {
            _context = context;
        }

    }
}
