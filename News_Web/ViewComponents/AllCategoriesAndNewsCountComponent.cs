using Microsoft.AspNetCore.Mvc;
using News_Business.Repository.IRepository;

namespace News_Web.ViewComponents
{
    public class AllCategoriesAndNewsCountComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public AllCategoriesAndNewsCountComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var category = await _categoryRepository.GetAllCategoryAndNewsCount();
            return View("AllCategoriesAndNewsCountViewComponent", category);
        }
    }
}
