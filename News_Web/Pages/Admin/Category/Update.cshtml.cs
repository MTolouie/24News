using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace News_Web.Pages.Admin.Category
{
    [Authorize(Roles = SD.Role_Admin)]
    public class UpdateModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public CategoryDTO categoryDTO { get; set; }


        public async Task OnGet(int CatId)
        {
            categoryDTO = await _categoryRepository.GetCategoryById(CatId);
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var UpdatedCategory = await _categoryRepository.UpdateCategory(categoryDTO.CatId,categoryDTO);
                if (UpdatedCategory != null)
                {
                    TempData[SD.Success] = $"{UpdatedCategory.CatTitle} Updated Successfuly";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData[SD.Error] = "SomeThing Went Wrong During Editing";
                    return RedirectToPage("Index");
                }
            }
            else
            {
                TempData[SD.Error] = "Form Is Not Valid";
                return RedirectToPage("Index");
            }
            return RedirectToPage("Index");
        }
    }
}
