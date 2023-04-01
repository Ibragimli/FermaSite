using Ferma.Core.Entites;
using Ferma.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Areas.Manage.ViewModels
{
    public class SubCategoryViewModel
    {
        public PagenetedList<SubCategory> SubCategories { get; set; }
        public List<Category> Categories { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
