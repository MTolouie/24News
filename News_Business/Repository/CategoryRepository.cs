using AutoMapper;
using Microsoft.EntityFrameworkCore;
using News_Business.Repository.IRepository;
using News_DataLayer.Data;
using News_DataLayer.Models;
using News_Models.DTOs;
using News_Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace News_Business.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            try
            {
                var category = await _db.Categories.ToListAsync();
                var categorydto = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(category);
                return categorydto;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<CategoryDTO> GetCategoryById(int CatId)
        {
            try
            {
                var category = await _db.Categories.SingleOrDefaultAsync(c => c.CatId == CatId);
                if (category == null)
                {
                    return null;
                }
                var categoryDto = _mapper.Map<Category, CategoryDTO>(category);
                return categoryDto;
            }
            catch (Exception e)
            {
                return null;
            }
        }



        public async Task<CategoryDTO> UpdateCategory(int CatId, CategoryDTO categoryDTO)
        {
            try
            {
                if (CatId == categoryDTO.CatId)
                {
                    var categoryFromDb = await _db.Categories.SingleOrDefaultAsync(c => c.CatId == CatId);
                    var category = _mapper.Map<CategoryDTO, Category>(categoryDTO, categoryFromDb);
                    var updatedCategory = _db.Categories.Update(category);
                    await _db.SaveChangesAsync();
                    return _mapper.Map<Category, CategoryDTO>(updatedCategory.Entity);

                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<bool> DeleteCategoryById(int CatId)
        {
            try
            {
                var category = await _db.Categories.FindAsync(CatId);
                if (category == null)
                {
                    return false;
                }
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> IsCatNameUnique(string name)
        {
            try
            {
                var catName = await _db.Categories.SingleOrDefaultAsync(c => c.CatTitle.ToLower() == name.ToLower());
                if (catName == null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> CreateCategory(CategoryDTO categoryDTO)
        {
            try
            {
                var category = _mapper.Map<CategoryDTO, Category>(categoryDTO);
                _db.Categories.Add(category);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<int> GetCategoryCount()
        {
            try
            {
                var CategoryCount = await _db.Categories.CountAsync();
                return CategoryCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        

        public async Task<CategoryDTO> GetRandomCategory()
        {

            try
            {
                var categories = await GetAllCategories();

                List<string> CategoriesName = new List<string>();
                Random rnd = new Random();
                foreach (var category in categories)
                {
                    CategoriesName.Add(category.CatTitle);
                }
                var index = rnd.Next(CategoriesName.Count);
                var randomCategoryName = CategoriesName[index];
                var categoryObj = await GetCategoryByTitle(randomCategoryName);
                return categoryObj;


            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<CategoryDTO> GetCategoryByTitle(string title)
        {
            try
            {
                var category = await _db.Categories.SingleOrDefaultAsync(c=>c.CatTitle.ToLower() == title.ToLower());
                if (category == null)
                {
                    return null;
                }
                return _mapper.Map<Category,CategoryDTO>(category);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<string> GetCategoryTitleById(int Id)
        {
            try
            {
                var category = await _db.Categories.FindAsync(Id);
                if (category == null)
                {
                    return null;
                }
                return category.CatTitle;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<CategoriesAndNewsCountViewModel> GetAllCategoryAndNewsCount()
        {
            try
            {
                var categories = await _db.Categories.ToListAsync();
                var newsCatIds = await _db.News.Select(s=>s.CatId).ToListAsync();
                CategoriesAndNewsCountViewModel ViewModel = new CategoriesAndNewsCountViewModel()
                {
                    Categories = _mapper.Map<List<Category>,List<CategoryDTO>>(categories),
                    NewsCatIds = newsCatIds
                };

                return ViewModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
