using Ferma.Core.Entites;
using Ferma.Service.Dtos.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IPosterCreateValueCheckServices
    {
        void CheckDescribe(string describe);

        void SubCategoryValidation(int subCategoryId);
        void CityValidation(int cityId);
        void ImageCheck(List<IFormFile> images);
    }
}
