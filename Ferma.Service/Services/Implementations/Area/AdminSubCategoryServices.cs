using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class AdminSubCategoryServices : IAdminSubCategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminSubCategoryServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task SubCategoryCreate(SubCategory SubCategory)
        {
            SubCategory newSubCategory = new SubCategory();
            bool check = false;
            if (SubCategory.Name != null)
            {
                if (await _unitOfWork.SubCategoryRepository.IsExistAsync(x => x.Name == SubCategory.Name))
                    throw new ItemAlreadyException("Bu adda altkategoriya mövcuddur!");

                if (SubCategory.Name.Length > 100)
                    throw new ItemFormatException("Şəhərin adı maksimum  uzunluğu 100 ola bilər");
                newSubCategory.Name = SubCategory.Name;
                check = true;
            }
            else
                throw new ItemNullException("Altkategoriya adı boş ola bilməz");

            if (SubCategory.CategoryId != 0)
            {
                newSubCategory.CategoryId = SubCategory.CategoryId;
                check = true;
            }
            else
                throw new ItemNullException("Kategoriya adı boş ola bilməz");

            if (check)
            {
                await _unitOfWork.SubCategoryRepository.InsertAsync(newSubCategory);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SubCategoryEdit(SubCategory subCategory)
        {
            bool check = false;
            var oldSubCategory = await _unitOfWork.SubCategoryRepository.GetAsync(x => x.Id == subCategory.Id);

            if (subCategory.Name != null)
            {
                if (await _unitOfWork.SubCategoryRepository.IsExistAsync(x => x.Name == subCategory.Name))
                    throw new ItemAlreadyException("Bu adda altkategoriya mövcuddur!");

                if (subCategory.Name.Length > 100)
                    throw new ItemFormatException("AltKategoriya adının maksimum  uzunluğu 100 ola bilər");
                if (subCategory.Name != oldSubCategory.Name)
                {
                    oldSubCategory.Name = subCategory.Name;
                    check = true;
                }
            }
            else
                throw new ItemNullException("AltKategoriya boş ola bilməz");
            if (subCategory.CategoryId != 0)
            {
                oldSubCategory.CategoryId = subCategory.CategoryId;
                check = true;
            }
            else
                throw new ItemNullException("Kategoriya adı boş ola bilməz");


            if (check)
            {
                oldSubCategory.ModifiedDate = DateTime.UtcNow.AddHours(4);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SubCategoryDelete(int id)
        {
            var oldSubCategory = await _unitOfWork.SubCategoryRepository.GetAsync(x => x.Id == id);
            bool check = await _unitOfWork.PosterFeaturesRepository.IsExistAsync(x => x.SubCategoryId == id);
            if (check)
                throw new ItemUseException("Altkategoriya elanda istifadə olunur!!!");

            _unitOfWork.SubCategoryRepository.Remove(oldSubCategory);
            await _unitOfWork.CommitAsync();
        }
        public async Task<SubCategory> GetSubCategory(int id)
        {
            var SubCategory = await _unitOfWork.SubCategoryRepository.GetAsync(x => x.Id == id && !x.IsDelete, "Category");

            return SubCategory;
        }
        public IQueryable<SubCategory> GetSubCategorys(string category, string subCategory)
        {
            var SubCategory = _unitOfWork.SubCategoryRepository.asQueryable();
            SubCategory = SubCategory.Where(x => !x.IsDelete);
            if (category != null)
                SubCategory = SubCategory.Where(i => EF.Functions.Like(i.Category.Name, $"%{category}%"));
            if (subCategory != null)
                SubCategory = SubCategory.Where(i => EF.Functions.Like(i.Name, $"%{subCategory}%"));

            return SubCategory;
        }
        public async Task<List<Category>> GetCategories()
        {
            var category = await _unitOfWork.CategoryRepository.GetAllAsync(x => !x.IsDelete);
            return category.ToList();
        }
    }
}
