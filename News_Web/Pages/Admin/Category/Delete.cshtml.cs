using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace News_Web.Pages.Admin.Category
{
    [Authorize(Roles = SD.Role_Admin)]
    public class DeleteModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public CategoryDTO CategoryDTO { get; set; }

        public async Task OnGet(int CatId)
        {
            CategoryDTO = await _categoryRepository.GetCategoryById(CatId);
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData[SD.Error] = "Something Went Wrong";
                return RedirectToPage("Index");
            }
            var IsDeleted = await _categoryRepository.DeleteCategoryById(CategoryDTO.CatId);
            if (IsDeleted)
            {
                TempData[SD.Success] = $"{CategoryDTO.CatTitle} Deleted Successfuly";
                return RedirectToPage("Index");
            }
            else
            {
                TempData[SD.Success] = "Failed";
                return RedirectToPage("Index");
            }
        }
    }
}
