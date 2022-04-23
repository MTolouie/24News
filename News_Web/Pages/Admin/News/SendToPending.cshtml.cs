using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;

namespace News_Web.Pages.Admin.News
{
    [Authorize(Roles = SD.Role_Admin)]
    public class SendToPendingModel : PageModel
    {
        private readonly INewsRepository _newsRepository;
        private readonly IPendingNewsRepository _pendingNewsRepository;
        private readonly IUserRepository _userRepository;

        public SendToPendingModel(INewsRepository newsRepository, IPendingNewsRepository pendingNewsRepository, IUserRepository userRepository)
        {

            _newsRepository = newsRepository;
            _pendingNewsRepository = pendingNewsRepository;
            userRepository = userRepository;

        }

        public NewsDTO NewsDTO { get; set; }
        //public ApplicationUserDTO UserDTO{ get; set; }

        [BindProperty]
        public PendingNewsDTO PendingDTO { get; set; }

        public async Task OnGet(int NewsId)
        {
            NewsDTO = await _newsRepository.GetNewsById(NewsId);

        }

        public async Task<IActionResult> OnPost()
        {

            if (ModelState.IsValid)
            {
                if (PendingDTO.NewsId == null)
                {
                    TempData[SD.Error] = "Something Went Wrong";
                    return RedirectToPage("UnPublished");
                }
                var news = await _newsRepository.GetNewsById(PendingDTO.NewsId);
                if (news == null)
                {
                    TempData[SD.Info] = "News Not found";
                    return NotFound();
                }

                news.IsPending = true;
                var UpdatedNews = await _newsRepository.UpdateNews(news.NewsId, news);
                if (UpdatedNews != null)
                {
                    TempData[SD.Success] = $"{news.NewsTitle} Sent To Pending";
                    
                }
                else
                {
                    TempData[SD.Error] = "Something Went Wrong";
                    return Page();
                }
                var htmlSanitizer = new HtmlSanitizer();
                var cleanText = htmlSanitizer.Sanitize(PendingDTO.Message);
                PendingDTO.Message = cleanText;

                var state = await _pendingNewsRepository.SendToPendingNews(PendingDTO);
                if (state)
                {
                    TempData[SD.Success] = "Message Sent";
                    return RedirectToPage("UnPublished");
                }
                else
                {
                    TempData[SD.Error] = "Something Went Wrong";
                    return RedirectToPage("UnPublished");
                }

            }
            return RedirectToPage("UnPublished");

        }
    }
}
