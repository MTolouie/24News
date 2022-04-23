using Microsoft.AspNetCore.Mvc;
using News_Business.Repository.IRepository;

namespace News_Web.ViewComponents
{
    public class TopNavComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public TopNavComponent( ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return View("TopNavViewComponent", categories);
        }
    }
}
