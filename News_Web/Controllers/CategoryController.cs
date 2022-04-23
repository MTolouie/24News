using Microsoft.AspNetCore.Mvc;
using News_Business.Repository.IRepository;

namespace News_Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly INewsRepository _newsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public CategoryController(INewsRepository newsRepository, ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _newsRepository = newsRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index(int CatId,int pageId = 1)
        {
            var news = await _newsRepository.GetNewsByCategoryIdForCategoryIndex(CatId, pageId);
            ViewData["CatId"] = CatId;
            ViewData["PageId"] = pageId;
            return View(news);
        }
    }
}
