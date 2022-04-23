using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;
using News_Web.IService;
using Microsoft.AspNetCore.Authorization;

namespace News_Web.Pages.Admin.News
{
    [Authorize(Roles = SD.Role_Admin)]
    public class DeleteModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IFileUpload _fileUpload;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteModel(ICategoryRepository categoryRepository, INewsRepository newsRepository, IFileUpload fileUpload, IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _newsRepository = newsRepository;
            _fileUpload = fileUpload;
            _httpContextAccessor = httpContextAccessor;
        }

        
        public NewsDTO NewsDTO { get; set; }

        public CategoryDTO CategoryDTO { get; set; }

        public async Task OnGet(int NewsId)
        {
            NewsDTO = await _newsRepository.GetNewsById(NewsId);
            CategoryDTO = await _categoryRepository.GetCategoryById(NewsDTO.CatId);
            
        }

        public async Task<IActionResult> OnPost(int NewsId,string imgUp)
        {
            CategoryDTO = await _categoryRepository.GetCategoryById(NewsId);
            if (NewsId != null)
            {
                var news = await _newsRepository.GetNewsById(NewsId);
                if (news != null)
                {
                    var Url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}//images/NewsImages/";
                    string filename = imgUp.Replace(Url, null);
                    var IsImgDeleted = await _fileUpload.DeleteFile(filename,SD.NewsImageType);
                    if (IsImgDeleted)
                    {
                        TempData[SD.Success] = "Image Deleted Successfuly";
                    }
                    else
                    {
                        TempData[SD.Error] = "Image Not Deleted";
                        return RedirectToPage("Archive");
                    }
                    var IsNewsDeleted = await _newsRepository.DeleteNewsById(NewsId);
                    if (IsNewsDeleted)
                    {
                        TempData[SD.Success] = "News Deleted Successfuly";
                        return RedirectToPage("Archive");
                    }
                    else
                    {
                        TempData[SD.Error] = "Could Not Delete News";
                        return RedirectToPage("Archive");
                    }
                }
                else
                {
                    TempData[SD.Error] = "Not Found";
                    return NotFound();
                }
            }
            else
            {
                TempData[SD.Error] = "Something Went Wrong";
                return RedirectToPage("Archive");
            }
        }
    }
}
