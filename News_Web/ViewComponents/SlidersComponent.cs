using Microsoft.AspNetCore.Mvc;
using News_Business.Repository.IRepository;

namespace News_Web.ViewComponents
{
    public class SlidersComponent : ViewComponent
    {
        private readonly INewsRepository _newsRepository;

        public SlidersComponent(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var SliderNews = await _newsRepository.GetNewsForSliders();
            return View("SlidersViewComponent", SliderNews.Take(2));
        }
    }
}
