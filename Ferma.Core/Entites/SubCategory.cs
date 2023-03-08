using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Core.Entites
{
    public class SubCategory : BaseEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<PosterFeatures> PosterFeatures { get; set; }

    }
}
