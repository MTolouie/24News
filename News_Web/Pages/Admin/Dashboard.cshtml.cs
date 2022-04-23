using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using News_Common;
using System.Web.Helpers;

namespace News_Web.Pages.Admin
{
    [Authorize(Roles = SD.Role_Admin)]
    public class DashboardModel : PageModel
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IUserRepository _userRepository;

        public DashboardModel(ICategoryRepository categoryRepository, INewsRepository newsRepository,IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _newsRepository = newsRepository;
            _userRepository = userRepository;
        }

        public int CategoryCount { get; set; }
        public int NewsCount{ get; set; }
        public int UnpublishedNewsCount{ get; set; }
        public int AllUsersCount { get; set; }
        public int ArchivedNewsCount { get; set; }
        public IEnumerable<CategoryDTO> Categories{ get; set; }

        public async Task OnGet()
        {
            NewsCount = await _newsRepository.GetNewsCount();
            CategoryCount = await _categoryRepository.GetCategoryCount();
            AllUsersCount = _userRepository.GetAllUserCount();
            //JournalistsCount = _userRepository.GetJournalistCount();
            Categories = await _categoryRepository.GetAllCategories();
            UnpublishedNewsCount = await _newsRepository.GetUnPublishedNewsCount();
            if (UnpublishedNewsCount > 0)
            {
                TempData[SD.Info] = $"You Have {UnpublishedNewsCount} UnPublished News To Publish";
            }
            
        }
    }
}
