using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ferma.Core.Entites
{
    public class Poster : BaseEntity
    {
        public int PosterFeaturesId { get; set; }
        public PosterFeatures PosterFeatures { get; set; }
        public ICollection<PosterUserId> PosterUserIds { get; set; }
        public ICollection<WishItem> WishItems { get; set; }
        public ICollection<PosterImage> PosterImages { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFiles { get; set; }
        [NotMapped]
        public IFormFile PosterImageFile { get; set; }
        [NotMapped]
        public List<int> PosterImagesIds { get; set; }


    }
}
