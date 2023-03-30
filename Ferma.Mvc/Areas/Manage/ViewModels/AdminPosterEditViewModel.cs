using Ferma.Core.Entites;
using Ferma.Service.Dtos.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Areas.Manage.ViewModels
{
    public class AdminPosterEditViewModel
    {
        public Poster Poster { get; set; }
        public AppUser AppUser { get; set; }
        public PosterEditPostDto PosterEditPostDto { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Category> Categories { get; set; }
        public List<City> Cities { get; set; }

    }
}
