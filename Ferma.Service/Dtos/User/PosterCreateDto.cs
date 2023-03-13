using Ferma.Core.Entites;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Dtos.User
{
    public class PosterCreateDto
    {

        public int SubCategoryId { get; set; }
        public string PosterName { get; set; }
        public decimal Price { get; set; }
        public bool PriceCurrency { get; set; }
        public int CityId { get; set; }
        public string Describe { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> ImageFilesStr { get; set; }
        public PosterImageFiles PosterImageFiles { get; set; }

    }
    public class PosterImageFiles
    {
        public List<IFormFile> ImageFiles { get; set; }
    }
}
