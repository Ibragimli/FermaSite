using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ferma.Core.Entites
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        [NotMapped]
        public IFormFile KeyImageFile { get; set; }

        [NotMapped]
        public ICollection<IFormFile> KeyImagesFiles { get; set; }
    }
}
