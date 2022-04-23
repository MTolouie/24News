using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using News_Common;

namespace News_Web.Pages.Admin.Category
{
    [Authorize(Roles = SD.Role_Admin)]
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public IndexModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<CategoryDTO> categoryDTO { get; set; }

        public async Task OnGet()
        {
            categoryDTO = await _categoryRepository.GetAllCategories();
        }
    }
}
