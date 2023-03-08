using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Dtos.User
{
    public class PosterCreateDto
    {
        public Poster Poster { get; set; }
        public PosterFeatures PosterFeatures { get; set; }

        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string PosterName { get; set; }
        public decimal Price { get; set; }
        public bool PriceCuurency { get; set; }
        public int CityId { get; set; }
        public string Describe { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
