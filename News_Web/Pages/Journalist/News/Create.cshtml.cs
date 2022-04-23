using Ganss.XSS;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;
using News_Web.IService;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace News_Web.Pages.Journalist.News
{
    [Authorize(Roles = SD.Role_Journalist)]
    public class CreateModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileUpload _fileUpload;

        public CreateModel(ICategoryRepository categoryRepository, INewsRepository newsRepository, IUserRepository userRepository, IFileUpload fileUpload)
        {
            _categoryRepository = categoryRepository;
            _newsRepository = newsRepository;
            _userRepository = userRepository;
            _fileUpload = fileUpload;
        }

        [BindProperty]
        public NewsDTO NewsDTO { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public async Task OnGet()
        {
            
            Categories = await _categoryRepository.GetAllCategories();
            var Name = User.Identity.Name;
            var userDto =  _userRepository.GetUserByUserName(Name);
            ViewData["UserId"] = userDto.Id;
        }


        public async Task<IActionResult> OnPost(IFormFile? imgUp)
        {
            var Name = User.Identity.Name;
            var userDto = _userRepository.GetUserByUserName(Name);
            ViewData["UserId"] = userDto.Id;
            Categories = await _categoryRepository.GetAllCategories();

            var htmlSanitizer = new HtmlSanitizer();
            var cleanText = htmlSanitizer.Sanitize(NewsDTO.Text);
            var cleanShortDesc = htmlSanitizer.Sanitize(NewsDTO.ShortDesc);
            NewsDTO.Text = cleanText;
            NewsDTO.ShortDesc= cleanShortDesc;

           
            if (ModelState.IsValid)
            {

                if (imgUp == null)
                {
                    NewsDTO.Images = "https://localhost:44395/images/NewsImages/no_photo.png";
                }
                else
                {
                    var imageFullUrl = await _fileUpload.UploadFile(imgUp,SD.NewsImageType);
                    if (imageFullUrl == null)
                    {
                        TempData[SD.Error] = "Image Format Must Be png Or Jpg Or Jpeg";
                        return Page();
                    }
                    NewsDTO.Images = imageFullUrl;
                }
                var isCreated = await _newsRepository.CreateNews(NewsDTO);
                if (isCreated)
                {
                    TempData[SD.Success] = "Created Successfuly";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData[SD.Error] = "Something Went Wrong";
                    return Page();
                }
            }
            else
            {
                //var modelErrors = new List<string>();
                //foreach (var modelState in ModelState.Values)
                //{
                //    foreach (var modelError in modelState.Errors)
                //    {
                //        modelErrors.Add(modelError.ErrorMessage);
                //        foreach (var item in modelErrors)
                //        {
                //            TempData[SD.Error] = $"{item}";
                //        }
                //    }
                //}
                TempData[SD.Error] = "Form Is Not Valid";
            }
            return Page();
        }

    }

}

