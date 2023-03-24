using Ferma.Core.Entites;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.HelperService.Interfaces
{
    public interface IManageImageHelper
    {
        public void PosterCheck(IFormFile posterImageFile);
        public void ImagesCheck(List<IFormFile> Images);
        public string FileSave(IFormFile Image, string folderName);
        public void DeleteFile(string image, string folderName);
    }
}
