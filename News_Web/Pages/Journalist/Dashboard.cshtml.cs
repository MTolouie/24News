using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;
using System.Web.Helpers;

namespace News_Web.Pages.Journalist
{
    [Authorize(Roles = SD.Role_Journalist)]
    public class DashboardModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPendingNewsRepository _pendingRepository;

        public DashboardModel(ICategoryRepository categoryRepository, INewsRepository newsRepository, IUserRepository userRepository, IPendingNewsRepository pendingNewsRepository)
        {
            _categoryRepository = categoryRepository;
            _newsRepository = newsRepository;
            _userRepository = userRepository;
            _pendingRepository = pendingNewsRepository;
        }

        public int PublishedNewsCount { get; set; }
        public int UnPublishedNewsCount { get; set; }
        public int ArchivedNewsCount { get; set; }
        public int NewsViewCount { get; set; }
        public IEnumerable<NewsDTO> DataNews { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public async void OnGet()
        {
            var username = User.Identity.Name;
            var user = _userRepository.GetUserByUserName(username);
            var PendingNewsCount =  _newsRepository.GetUserPendingNewsCount(user.Id);
            if (PendingNewsCount > 0)
            {
                TempData[SD.Info] = $"You Have {PendingNewsCount} Pending News That Need Updating";
            }
            PublishedNewsCount = await _newsRepository.GetUserNewsCount(user.Id);
            UnPublishedNewsCount =  _newsRepository.GetUserUnPublishedNewsCount(user.Id);
            NewsViewCount =  _newsRepository.GetUserNewsViewCount(user.Id);
            ArchivedNewsCount = await _newsRepository.GetUserArchivedNewsCount(user.Id);
            DataNews =  _newsRepository.GetUserTopNewsByViewCount(user.Id);
            





        }
    }
}
