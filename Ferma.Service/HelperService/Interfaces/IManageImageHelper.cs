using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.HelperService.Interfaces
{
    public interface IManageImageHelper
    {
        public void PosterCheck(Poster Image);
        public void ImagesCheck(Poster Images);
        public string FileSave(Poster Image, string folderName);
        public void DeleteFile(string image, string folderName);
    }
}
