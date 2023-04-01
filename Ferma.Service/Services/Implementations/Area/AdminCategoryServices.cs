using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class AdminCategoryServices : IAdminCategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManageImageHelper _manageImageHelper;

        public AdminCategoryServices(IUnitOfWork unitOfWork, IManageImageHelper manageImageHelper)
        {
            _unitOfWork = unitOfWork;
            _manageImageHelper = manageImageHelper;
        }
        public async Task CategoryCreate(Category category)
        {
            Category newCategory = new Category();

            bool check = false;
            if (category.CategoryImageFile != null)
                _manageImageHelper.PosterCheck(category.CategoryImageFile);

            if (category.Name != null)
            {
                if (category.Name.Length > 50)
                    throw new ItemFormatException("Kategoriyanın maksimum  uzunluğu 50 ola bilər");
                newCategory.Name = category.Name;
                check = true;
            }
            else
                throw new ItemNullException("Kategoriya boş ola bilməz");


            string imageFilename = PosterImageCreate(category);

            if (imageFilename != "false")
            {
                newCategory.Image = imageFilename;
                check = true;
            }

            if (check)
            {
                await _unitOfWork.CategoryRepository.InsertAsync(newCategory);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task CategoryEdit(Category category)
        {
            bool check = false;
            var oldCategory = await _unitOfWork.CategoryRepository.GetAsync(x => x.Id == category.Id);
            if (category.CategoryImageFile != null)
                _manageImageHelper.PosterCheck(category.CategoryImageFile);

            if (category.Name != null)
            {
                if (category.Name.Length > 50)
                    throw new ItemFormatException("Kategoriyanın maksimum  uzunluğu 50 ola bilər");
                if (category.Name != oldCategory.Name)
                {
                    oldCategory.Name = category.Name;
                    check = true;
                }
            }
            else
                throw new ItemNullException("Kategoriya boş ola bilməz");


            string imageFilename = PosterImageChange(category, oldCategory);

            if (imageFilename != "false")
            {
                oldCategory.Image = imageFilename;
                check = true;
            }

            if (check)
            {
                oldCategory.ModifiedDate = DateTime.UtcNow.AddHours(4);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task CategoryDelete(int id)
        {
            var oldCategory = await _unitOfWork.CategoryRepository.GetAsync(x => x.Id == id);
            bool check = await _unitOfWork.SubCategoryRepository.IsExistAsync(x => x.CategoryId == id);
            if (check)
                throw new ItemAlreadyException("Kategoriya altkateqoriyalarda istifadə olunur!!!");
            PosterImageDelete(oldCategory);


            _unitOfWork.CategoryRepository.Remove(oldCategory);
            await _unitOfWork.CommitAsync();
        }
        private void PosterImageDelete(Category oldCategory)
        {
            var Image = oldCategory.Image;
            if (Image == null) throw new ImageNullException("Şəkil tapılmadı!");
            _manageImageHelper.DeleteFile(Image, "category");
        }
        private string PosterImageCreate(Category category)
        {
            if (category.CategoryImageFile != null)
            {
                var posterImageFile = category.CategoryImageFile;
                string filename = _manageImageHelper.FileSave(posterImageFile, "category");
                return filename;
            }
            else
                throw new ImageNullException("Şəkil hissəsi boş ola bilməz!");
        }
        private string PosterImageChange(Category category, Category oldCategory)
        {
            if (category.CategoryImageFile != null)
            {
                var posterImageFile = category.CategoryImageFile;

                var Image = oldCategory.Image;

                if (Image == null) throw new ImageNullException("Şəkil tapılmadı!");

                string filename = _manageImageHelper.FileSave(posterImageFile, "category");
                _manageImageHelper.DeleteFile(Image, "category");
                Image = filename;

                return filename;
            }
            return "false";
        }
        public async Task<Category> GetCategory(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(x => x.Id == id && !x.IsDelete);

            return category;
        }
        public IQueryable<Category> GetCategories(string name)
        {
            var category = _unitOfWork.CategoryRepository.asQueryable();
            category = category.Where(x => !x.IsDelete);
            if (name != null)
                category = category.Where(i => EF.Functions.Like(i.Name, $"%{name}%"));

            return category;
        }
    }
}
