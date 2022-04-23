using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;
using News_Web.IService;
using Microsoft.AspNetCore.Authorization;

namespace News_Web.Pages.Journalist.News
{
    [Authorize(Roles = SD.Role_Journalist)]
    public class UpdateModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IFileUpload _fileUpload;
        private readonly IUserRepository _userRepository;
        private readonly IPendingNewsRepository _pendingNewsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateModel(IPendingNewsRepository pendingNewsRepository,ICategoryRepository categoryRepository, INewsRepository newsRepository, IFileUpload fileUpload, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _newsRepository = newsRepository;
            _fileUpload = fileUpload;
            _userRepository = userRepository;
            _pendingNewsRepository = pendingNewsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public NewsDTO NewsDTO { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }

        public async Task<IActionResult> OnGet(int NewsId, string UserId)
        {
            var Name = User.Identity.Name;
            var userDto = _userRepository.GetUserByUserName(Name);
            if (userDto.Id != UserId)
            {
                TempData[SD.Error] = "You Are Not The Author Of This News";
                return RedirectToPage("Index");
            }

            NewsDTO = await _newsRepository.GetNewsById(NewsId);
            Categories = await _categoryRepository.GetAllCategories();
            return Page();
        }

        public async Task<IActionResult> OnPost(IFormFile? imgUp)
        {
            Categories = await _categoryRepository.GetAllCategories();
            if (ModelState.IsValid)
            {
                if (imgUp != null)
                {
                    var Url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}//images/NewsImages/";
                    
                    if (NewsDTO.Images.Replace(Url, null) != "no_photo.png")
                    {
                        var state = await _fileUpload.DeleteFile(NewsDTO.Images.Replace(Url, null), SD.NewsImageType);
                        if (state)
                        {
                            TempData[SD.Success] = "Image Deleted Successfuly";
                        }
                        else
                        {
                            TempData[SD.Error] = "Something Went Wrong During Image Deleting";
                            return Page();
                        }
                    }
                   
                    NewsDTO.Images = await _fileUpload.UploadFile(imgUp, SD.NewsImageType);
                }

                var htmlSanitizer = new HtmlSanitizer();
                var cleanText = htmlSanitizer.Sanitize(NewsDTO.Text);
                var cleanShortDesc = htmlSanitizer.Sanitize(NewsDTO.ShortDesc);
                NewsDTO.Text = cleanText;
                NewsDTO.ShortDesc = cleanShortDesc;

                var pendingNews = _pendingNewsRepository.GetPendingNews(NewsDTO.NewsId,NewsDTO.UserId);
                if (pendingNews != null)
                {
                    var IsDeleted = _pendingNewsRepository.DeletePendingNews(NewsDTO.NewsId, NewsDTO.UserId);

                }

                var UpdatedNews = await _newsRepository.UpdateNews(NewsDTO.NewsId, NewsDTO);
                //if (UpdatedNews != null)
                //{
                TempData[SD.Success] = $"{UpdatedNews.NewsTitle} Updated Successfuly";
                return RedirectToPage("Index");
                //}
                //else
                //{
                //    TempData[SD.Success] = "Update Failed";
                //    return Page();
                //}
            }
            else
            {
                TempData[SD.Error] = "Form Is Not Valid";
                return Page();
            }

        }
    }
}
