using Ferma.Core.Entites;
using Ferma.Data.Datacontext;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class LayoutServices : ILayoutServices
    {
        private readonly DataContext _context;

        public LayoutServices(DataContext context)
        {

        }
        public async Task<List<Setting>> GetSettingsAsync()
        {
            return await _context.Settings.ToListAsync();
        }
    }
}
