using Ferma.Core.Entites;
using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ViewModels
{
    public class PosterEditViewModel
    {
        public Poster Poster { get; set; }
        public List<City> Cities { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Category> Categories { get; set; }
        public PosterEditDto PosterEditDto { get; set; }
    }
}
