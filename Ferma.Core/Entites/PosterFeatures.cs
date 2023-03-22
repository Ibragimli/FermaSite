using Ferma.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Core.Entites
{
    public class PosterFeatures : BaseEntity
    {
        public string Name { get; set; }
        public string Describe { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal? Price { get; set; }
        public bool PriceCurrency { get; set; }
        public int ViewCount { get; set; }
        public int WishCount { get; set; }
        public bool IsVip { get; set; }
        public bool IsNew { get; set; }
        public bool IsShipping { get; set; }
        public bool IsPremium { get; set; }
        public bool IsDisabled { get; set; }
        public int SubCategoryId { get; set; }
        public int CityId { get; set; }
        public PosterStatus PosterStatus { get; set; }
        public SubCategory SubCategory { get; set; }
        public City City { get; set; }
        public ICollection<Poster> Posters { get; set; }

    }
}
