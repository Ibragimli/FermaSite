using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Dtos.User
{
    public class PosterEditGetDto
    {
        public Poster Poster { get; set; }
        public List<City> Cities { get; set; }
        public IEnumerable<SubCategory> SubCategories { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public PosterEditDto PosterEditDto { get; set; }
    }
}
