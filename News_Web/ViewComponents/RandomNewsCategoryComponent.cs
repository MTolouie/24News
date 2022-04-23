using Microsoft.AspNetCore.Mvc;
using News_Business.Repository.IRepository;
using News_Models.ViewModels;

namespace News_Web.ViewComponents
{
    public class RandomNewsCategoryComponent : ViewComponent
    {
        private readonly INewsRepository _newsRepository;
        private readonly ICategoryRepository _categoryRepository;

        public RandomNewsCategoryComponent(INewsRepository newsRepository, ICategoryRepository categoryRepository)
        {
            _newsRepository = newsRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var category = await _categoryRepository.GetRandomCategory();
            IEnumerable<NewsForIndexViewModel> newses = new List<NewsForIndexViewModel>();
            if (category != null)
            {
                newses = await _newsRepository.GetNewsByCategoryId(category.CatId);
            }
            return View("RandomNewsCategoryViewComponent", newses.Take(4));
        }
    }
}
