using Microsoft.AspNetCore.Mvc;
using News_Business.Repository.IRepository;

namespace News_Web.ViewComponents
{
    public class LastWeekBestNewsComponent : ViewComponent
    {
        private readonly INewsRepository _newsRepository;

        public LastWeekBestNewsComponent(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var newses = await _newsRepository.GetLastWeekBestNews();
            return View("LastWeekBestNewsViewComponent", newses.Take(4));
        }

    }
}
