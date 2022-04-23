using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;

namespace News_Web.Pages.Journalist.News
{
    public class ShowMessageModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPendingNewsRepository _pendingRepository;

        public ShowMessageModel(ICategoryRepository categoryRepository, INewsRepository newsRepository, IUserRepository userRepository, IPendingNewsRepository pendingNewsRepository)
        {
            _categoryRepository = categoryRepository;
            _newsRepository = newsRepository;
            _userRepository = userRepository;
            _pendingRepository = pendingNewsRepository;
        }

        public PendingNewsDTO Pending { get; set; }

        public async void OnGet(int NewsId,string UserId)
        {
            var username = User.Identity.Name;
            var user =  _userRepository.GetUserByUserName(username);
            if (user.Id != UserId)
            {
                TempData[SD.Error] = "You Are Not The Author Of This News";
                HttpContext.Response.Redirect("/journalist/news/pending");
            }
            Pending =  _pendingRepository.GetPendingNews(NewsId, UserId);
        }
    }
}
