using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;

namespace News_Web.Pages.Admin.News
{
    [Authorize(Roles = SD.Role_Admin)]
    public class SendToPublishModel : PageModel
    {
        private readonly INewsRepository _newsRepository;
        private readonly IPendingNewsRepository _pendingNewsRepository;
        private readonly IUserRepository _userRepository;

        public SendToPublishModel(INewsRepository newsRepository, IPendingNewsRepository pendingNewsRepository, IUserRepository userRepository)
        {

            _newsRepository = newsRepository;
            _pendingNewsRepository = pendingNewsRepository;
            userRepository = userRepository;

        }

        public NewsDTO NewsDTO { get; set; }

        public async Task OnGet(int NewsId)
        {

            NewsDTO = await _newsRepository.GetNewsById(NewsId);
            if (NewsDTO == null)
            {
                TempData[SD.Error] = "Wrong News";
                HttpContext.Response.Redirect("/Admin/News/Unpublished");
            }
        }

        public async Task<IActionResult> OnGetSendToPublishHandler(int NewsId)
        {
            var news = await _newsRepository.GetNewsById(NewsId);
            if (news == null)
            {
                TempData[SD.Error] = "News Not found";
                return NotFound();
            }
            news.IsPublished = true;
            var updatedNews = await _newsRepository.UpdateNews(news.NewsId,news);
            if (updatedNews != null)
            {
                TempData[SD.Success] = $"{news.NewsTitle} Published";
                return RedirectToPage("UnPublished");
            }
            else
            {
                TempData[SD.Error] = "Something Went Wrong";
                return RedirectToPage("UnPublished");
            }
        }
    }
}
