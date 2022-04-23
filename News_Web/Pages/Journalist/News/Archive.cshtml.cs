using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using News_Common;

namespace News_Web.Pages.Journalist.News
{
    [Authorize(Roles = SD.Role_Journalist)]
    public class ArchiveModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IUserRepository _userRepository;

        public ArchiveModel(ICategoryRepository categoryRepository, INewsRepository newsRepository, IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _newsRepository = newsRepository;
            _userRepository = userRepository;
        }

        public Tuple<List<NewsDTO>,int> NewsDTO { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }

        public async Task OnGet(int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "")
        {
            ViewData["PageId"] = pageId;
            ViewData["Title"] = Title;
            ViewData["FromDate"] = FromDate;
            ViewData["ToDate"] = ToDate;
            if (Category != 0)
            {
                ViewData["Category"] = await _categoryRepository.GetCategoryById(Category);
            }
            else
            {
                ViewData["Category"] = "";
            }
            if (!string.IsNullOrEmpty(FromDate))
            {
                FromDate = FromDate.Replace("-", "/");
            }
            if (!string.IsNullOrEmpty(ToDate))
            {
                ToDate = ToDate.Replace("-", "/");
            }
            var username = User.Identity.Name;
            var user = _userRepository.GetUserByUserName(username);

            NewsDTO = await _newsRepository.GetUserArchivedNews(user.Id,Category, pageId, Title, FromDate, ToDate);
            Categories = await _categoryRepository.GetAllCategories();
        }
    }
}
