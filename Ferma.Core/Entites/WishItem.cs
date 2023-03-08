using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Core.Entites
{
    public class WishItem : BaseEntity
    {
        public int AppUserId { get; set; }
        public int PosterId { get; set; }
        public AppUser AppUser { get; set; }
        public Poster Poster { get; set; }
    }
}
