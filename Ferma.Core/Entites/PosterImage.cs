using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Core.Entites
{
    public class PosterImage : BaseEntity
    {
        public string Image { get; set; }
        public int PosterId { get; set; }
        public bool IsPoster { get; set; }
        public Poster Poster { get; set; }
    }
}
