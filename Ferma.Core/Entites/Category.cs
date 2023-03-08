using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ferma.Core.Entites
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public IFormFile CategoryImageFile { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
    }
}

