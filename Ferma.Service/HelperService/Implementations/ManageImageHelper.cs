using Ferma.Core.Entites;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Helper;
using Ferma.Service.HelperService.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.HelperService.Implementations
{
    public class ManageImageHelper : IManageImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly IImageValue _key;

        public ManageImageHelper(IWebHostEnvironment env, IImageValue key)
        {
            _env = env;
            _key = key;
        }
        public async void PosterCheck(Poster Image)
        {
            if (Image.PosterImageFile.ContentType != await _key.ValueStr("ImageType1") && Image.PosterImageFile.ContentType != await _key.ValueStr("ImageType2"))
                throw new ImageFormatException("Poster şekli yalnız (png ve ya jpg) type-ında ola biler");

            if (Image.PosterImageFile.Length > await _key.ValueInt("ImageSize") * 1048576)
                throw new ImageFormatException("Poster şeklinin max yaddaşı 2MB ola biler!");
        }
        public async void ImagesCheck(Poster Images)
        {
            foreach (var image in Images.ImageFiles)
            {
                if (image.ContentType != await _key.ValueStr("ImageType1") && image.ContentType != await _key.ValueStr("ImageType2"))
                    throw new ImageFormatException("Poster şekli yalnız (png ve ya jpg) type-ında ola biler");
                if (image.Length > await _key.ValueInt("ImageSize") * 1048576)
                    throw new ImageFormatException("Poster şeklinin max yaddaşı 2MB ola biler!");
            }
        }
        public string FileSave(Poster Image, string folderName)
        {
            string image = FileManager.Save(_env.WebRootPath, "uploads/folderName", Image.PosterImageFile);
            return image;
        }
        public void DeleteFile(string image, string folderName)
        {
            FileManager.Delete(_env.WebRootPath, "uploads/folderName", image);
        }
    }
}
