using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;

namespace News_Web.Pages.Admin.Category
{
    [Authorize(Roles = SD.Role_Admin)]
    public class CreateModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public CategoryDTO categoryDTO { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData[SD.Error] = "Something Went Wrong";
                return RedirectToPage("Index");

            }
            var NameDoesNotExist = await _categoryRepository.IsCatNameUnique(categoryDTO.CatTitle);

            if (NameDoesNotExist)
            {
                
                var IsCreated = await _categoryRepository.CreateCategory(categoryDTO);
                if (IsCreated)
                {
                    TempData[SD.Success] = "Category Created";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData[SD.Error] = "Something Went Wrong During Creating";
                    return RedirectToPage("Index");
                }
            }
            else
            {
                TempData[SD.Error] = "This Category Name Already Exists";
                ModelState.AddModelError("","This Category Name Already Exists");
                return RedirectToPage("Index");

            }

        }
    }
}
