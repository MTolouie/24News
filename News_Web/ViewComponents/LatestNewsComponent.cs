using Microsoft.AspNetCore.Mvc;
using News_Business.Repository.IRepository;

namespace News_Web.ViewComponents
{
    public class LatestNewsComponent : ViewComponent
    {
        private readonly INewsRepository _newsRepository;

        public LatestNewsComponent(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var newses = await _newsRepository.GetLatestNews();
            return View("LatestNewsViewComponent", newses);
        }
    }
}
