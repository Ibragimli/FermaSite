using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ViewModels
{
    public class ElanDetailViewModel
    {
        public Poster Poster { get; set; }
        public List<Poster> SimilarPosters { get; set; }
        public PosterUserId User { get; set; }
    }
}
