using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;

namespace News_Web.Pages.Journalist.News
{
    [Authorize(Roles =SD.Role_Journalist)]
    public class DetailsModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IUserRepository _userRepository;

        public DetailsModel(ICategoryRepository categoryRepository, INewsRepository newsRepository,IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _newsRepository = newsRepository;
            _userRepository = userRepository;
        }

        public NewsDTO NewsDTO { get; set; }
        public ApplicationUserDTO UserDTO{ get; set; }

        public async Task OnGet(int NewsId)
        {
            NewsDTO = await _newsRepository.GetNewsById(NewsId);
            UserDTO = await _userRepository.GetUserById(NewsDTO.UserId);
            if (NewsDTO.UserId != UserDTO.Id)
            {
                TempData[SD.Error] = "You Are Not The Author Of This News";
                HttpContext.Response.Redirect("/Journalist/News/Index");
            }
        }
    }
}
