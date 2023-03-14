using Ferma.Core.Entites;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IPosterCreateValueCheckServices
    {
        void PhoneNumberValidation(string phoneNumber);
        void SubCategoryValidation(int subCategoryId);
        void CityValidation(int cityId);
        void ImageCheck(List<IFormFile> images);
    }
}
