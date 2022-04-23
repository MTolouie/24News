using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_DataLayer.Models;
using News_Models.DTOs;
using News_Web.IService;

namespace News_Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ChangeAvatarModel : PageModel
    {
        private readonly IFileUpload _fileUpload;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public ChangeAvatarModel(IFileUpload fileUpload, IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _fileUpload = fileUpload;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public ApplicationUserDTO UserDTO { get; set; }

        public async Task OnGet()
        {
            var UserId = _userManager.GetUserId(User);
            UserDTO = await _userRepository.GetUserById(UserId);
            if (UserDTO.Avatar == null)
            {
                UserDTO.Avatar = "/images/UserImages/no_photo.png";
            }


        }

        public async Task<IActionResult> OnPost(IFormFile imgUp, string? CurrentAvatar, string userId)
        {

            UserDTO = await _userRepository.GetUserById(userId);
            if (UserDTO.Avatar == null)
            {
                UserDTO.Avatar = "/images/UserImages/no_photo.png";
            }



            var user = await _userRepository.GetUserById(userId);
            if (ModelState.IsValid)
            {


                if (CurrentAvatar == null)
                {
                    var UploadedfileName = await _fileUpload.UploadFile(imgUp, SD.UserImageType);
                    user.Avatar = UploadedfileName;
                    var status = await _userRepository.UpdateUserAvatar(userId, UploadedfileName);
                    if (status)
                    {
                        TempData[SD.Success] = "Image Edited Successfuly";
                        if (User.IsInRole(SD.Role_Admin))
                        {
                            return Redirect("/Admin/Dashboard");
                        }
                        else if (User.IsInRole(SD.Role_Journalist))
                        {
                            return Redirect("/Journalist/Dashboard");
                        }
                        else
                        {
                            return Redirect("/Identity/Account/Manage");
                        }
                    }
                    else
                    {
                        TempData[SD.Error] = "Something Went Wrong During Upload";

                        return Page();
                    }
                }
                else
                {

                    var fileName = CurrentAvatar.Replace("https://localhost:44395//images/UserImages/", null);
                    var IsDeleted = await _fileUpload.DeleteFile(fileName, SD.UserImageType);
                    if (IsDeleted)
                    {
                        var UploadedFileName = await _fileUpload.UploadFile(imgUp, SD.UserImageType);
                        user.Avatar = UploadedFileName;
                        var status = await _userRepository.UpdateUserAvatar(userId, UploadedFileName);
                        if (!status)
                        {
                            TempData[SD.Error] = "Something Went Wrong During Update";

                            return Page();
                        }
                        else
                        {
                            TempData[SD.Success] = "Image Edited Successfuly";
                            if (User.IsInRole(SD.Role_Admin))
                            {
                                return Redirect("/Admin/Dashboard");
                            }
                            else if (User.IsInRole(SD.Role_Journalist))
                            {
                                return Redirect("/Journalist/Dashboard");
                            }
                            else
                            {
                                return Redirect("/Identity/Account/Manage");
                            }
                        }
                    }
                    else
                    {
                        TempData[SD.Error] = "Something Went Wrong During Delete";

                        return Page();
                    }
                }


            }
            else
            {
                TempData[SD.Error] = "Form Was Not Valid";

                return Page();
            }

            return Page();
        }


        private async Task PrepareUserForGetMethod(string userId)
        {
            var UserDTO = await _userRepository.GetUserById(userId);
            if (UserDTO == null)
            {
                TempData[SD.Error] = "Something Went Wrong";

            }

            if (UserDTO.Avatar == null)
            {
                UserDTO.Avatar = "/images/UserImages/no_photo.png";

            }
        }
    }
}
