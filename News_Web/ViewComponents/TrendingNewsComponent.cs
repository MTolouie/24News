using Microsoft.AspNetCore.Mvc;
using News_Business.Repository.IRepository;

namespace News_Web.ViewComponents
{
    public class TrendingNewsComponent : ViewComponent
    {
        private readonly INewsRepository _newsRepository;

        public TrendingNewsComponent(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var trendingNews = await _newsRepository.GetTrendingNewsForIndex();
            return View("TrendingNewsViewComponent", trendingNews.Take(4));
        }
    }
}
