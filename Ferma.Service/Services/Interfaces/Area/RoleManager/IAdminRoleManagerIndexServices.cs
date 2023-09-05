﻿using Ferma.Core.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.Area.RoleManagers
{
    public interface IAdminRoleManagerIndexServices
    {
        public IQueryable<IdentityRole> GetRoleManager(string name);
    }
}
