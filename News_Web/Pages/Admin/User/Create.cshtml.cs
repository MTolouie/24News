using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Common;
using News_DataLayer.Models;
using News_Models.DTOs;
using News_Web.IService;
using System.ComponentModel.DataAnnotations;

namespace News_Web.Pages.Admin.User
{
    public class CreateModel : PageModel
    {
        private readonly IFileUpload _fileUpload;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(IFileUpload fileUpload, UserManager<IdentityUser> userManager)
        {
            _fileUpload = fileUpload;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} Is Required")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} Is Required")]
            [MaxLength(100)]
            [Display(Name = "Full Name")]
            public string Name { get; set; }


            [Required(ErrorMessage = "{0} Is Required")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(IFormFile? imgUp)
        {
            if (ModelState.IsValid)
            {
                var imgName = "https://localhost:44395/images/UserImages/no_photo.png";
                if (imgUp != null)
                {
                    imgName = await _fileUpload.UploadFile(imgUp, SD.UserImageType);
                }
                var DbUser = await _userManager.FindByEmailAsync(Input.Email);
                if (DbUser != null)
                {
                    TempData[SD.Error] = "This Email Is Taken";
                    return Page();
                }

                var user = new ApplicationUser
                {
                    UserName = Input.Email.Trim().ToLower(),
                    Email = Input.Email.Trim().ToLower(),
                    Name = Input.Name.Trim().ToLower(),
                    Avatar = imgName,
                    CreateDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, SD.Role_Journalist);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        TempData[SD.Success] = "CreatedSuccessfuly";

                        return RedirectToPage("Index");
                    }
                }
                else
                {
                    TempData[SD.Error] = "Something Went Wrong";
                    return Page();
                }


            }
            return Page();
        }
    }
}
