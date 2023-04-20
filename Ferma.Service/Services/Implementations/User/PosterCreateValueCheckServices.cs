using Ferma.Core.IUnitOfWork;
using Ferma.Data.Datacontext;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ferma.Service.Services.Implementations.User
{
    public class PosterCreateValueCheckServices : IPosterCreateValueCheckServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public PosterCreateValueCheckServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CheckDescribe(string describe)
        {
            if (describe == null)
            {
                throw new ItemNullException("Təsvir hissəsi boş ola bilməz!");
            }
        }
        public void SubCategoryValidation(int subCategoryId)
        {
            if (subCategoryId == 0)
                throw new ItemNullException("Kategoriyanı  qeyd edin");

            bool check = _unitOfWork.SubCategoryRepository.IsExist(x => x.Id == subCategoryId);
            if (!check)
                throw new ItemNotFoundException("Düzgün kategoriya seçilməyib");
        }


        public void ImageCheck(List<IFormFile> images)
        {
            if (images == null)
                throw new ImageNullException("Şəkil yüklənilməlidir");

        }

        public void CityValidation(int cityId)
        {
            bool check = _unitOfWork.CityRepository.IsExist(x => x.Id == cityId);
            if (!check)
                throw new ItemNotFoundException("Düzgün şəhər seçilməyib");
        }
    }
}
