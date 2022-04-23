using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace News_Web.Pages.Admin.News
{
    [Authorize(Roles = SD.Role_Admin)]
    public class SendToArchiveModel : PageModel
    {
        private readonly INewsRepository _newsRepository;

        public SendToArchiveModel(INewsRepository newsRepository)
        {

            _newsRepository = newsRepository;

        }

        public NewsDTO NewsDTO { get; set; }


        public async Task OnGet(int NewsId)
        {
            NewsDTO = await _newsRepository.GetNewsById(NewsId);
        }

        public async Task<IActionResult> OnGetSendToArchiveHandler(int NewsId)
        {
            if (NewsId == null)
            {
                TempData[SD.Error] = "Something Went Wrong";
                return RedirectToPage("Index");
            }
            var news = await _newsRepository.GetNewsById(NewsId);
            if (news == null)
            {
                TempData[SD.Info] = "News Not found";
                return NotFound();
            }
            if (news.IsArchived == false)
            {
                var state = await _newsRepository.ArchiveNewsById(NewsId);
                if (state)
                {
                    TempData[SD.Success] = "News Archived Successfuly";
                    return RedirectToPage("Archive");
                }
                else
                {
                    TempData[SD.Success] = "Failed";
                    return RedirectToPage("Index");
                }
            }
            else
            {
                var state = await _newsRepository.UnArchiveNewsById(NewsId);
                if (state)
                {
                    TempData[SD.Success] = "News UnArchived Successfuly";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData[SD.Success] = "Failed";
                    return RedirectToPage("Index");
                }
            }
            return RedirectToPage("Index");
        }
    }
}
