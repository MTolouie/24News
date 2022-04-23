using News_Models.DTOs;
using News_Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace News_Business.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<CategoryDTO>> GetAllCategories();
        public Task<CategoryDTO> GetRandomCategory();
        public Task<CategoryDTO> GetCategoryById(int CatId);
        public Task<CategoryDTO> GetCategoryByTitle(string title);
        public Task<string> GetCategoryTitleById(int Id);
        public Task<bool> CreateCategory(CategoryDTO categoryDTO);
        public Task<CategoryDTO> UpdateCategory(int CatId, CategoryDTO categoryDTO);
        public Task<bool> DeleteCategoryById(int CatId);
        public Task<bool> IsCatNameUnique(string name);
        public Task<int> GetCategoryCount();
        public Task<CategoriesAndNewsCountViewModel> GetAllCategoryAndNewsCount();
    }
}
