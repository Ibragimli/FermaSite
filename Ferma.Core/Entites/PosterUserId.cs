﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Core.Entites
{
    public class PosterUserId : BaseEntity
    {
        public string AppUserId { get; set; }
        public int PosterId { get; set; }
        public AppUser AppUser { get; set; }
        public Poster Poster { get; set; }
    }
}
