using Ferma.Core.Entites;
using Ferma.Core.Repositories;
using Ferma.Data.Datacontext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Data.Repositories
{
    public class UserAuthenticationRepository : Repository<UserAuthentication>, IUserAuthenticationRepository
    {
        private readonly DataContext _context;

        public UserAuthenticationRepository(DataContext context) : base(context)
        {
            _context = context;
        }

    }
}
