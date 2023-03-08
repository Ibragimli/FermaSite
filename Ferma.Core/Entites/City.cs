using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Core.Entites
{
   public class City:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<PosterFeatures> PosterFeatures { get; set; }

    }
}
