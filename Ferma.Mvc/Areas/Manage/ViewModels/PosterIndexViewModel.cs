using Ferma.Core.Entites;
using Ferma.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Areas.Manage.ViewModels
{
    public class PosterIndexViewModel
    {
        public PagenetedList<Poster> Posters { get; set; }
        public List<Category> Categories { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public PosterDeleteModal PosterDeleteModal { get; set; }
    }
    public class PosterDeleteModal
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
