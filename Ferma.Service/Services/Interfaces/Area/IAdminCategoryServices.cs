﻿using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.Area
{
    public interface IAdminCategoryServices
    {
        public IQueryable<Category> GetCategories(string name);
        public Task<Category> GetCategory(int id);
        public Task CategoryCreate(Category category);

        public Task CategoryEdit(Category category);
        public Task CategoryDelete(int id);


    }
}
