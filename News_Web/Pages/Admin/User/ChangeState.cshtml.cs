using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;

namespace News_Web.Pages.Admin.User
{
    [Authorize(Roles = SD.Role_Admin)]
    public class ChangeStateModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public ChangeStateModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ApplicationUserDTO UserDTO { get; set; }
        public async Task OnGet(string UserId)
        {
            UserDTO = await _userRepository.GetUserById(UserId);
        }

        public async Task<IActionResult> OnGetChangeStateHandler(string UserId)
        {
            if (UserId == null)
            {
                TempData[SD.Error] = "Something Went Wrong";
                return RedirectToPage("Index");
            }
            var user = await _userRepository.GetUserById(UserId);
            if (user == null)
            {
                TempData[SD.Error] = "User Not Found";
                return RedirectToPage("Index");
            }
            var userRole = _userRepository.GetUserRoleName(UserId);

            if (userRole == SD.Role_Admin)
            {
                TempData[SD.Error] = "You Cant Change Your State";
                return RedirectToPage("Index");
            }

            if (user.IsActive)
            {
                var state = await _userRepository.DeActivateUserById(UserId);
                if (state)
                {
                    TempData[SD.Success] = "User Successfuly DeActivated";
                    return RedirectToPage("DeActiveUsers");
                }
                else
                {
                    TempData[SD.Error] = "Something Went Wrong During The Operation";
                    return RedirectToPage("Index");
                }

            }
            else
            {
                var state = await _userRepository.ActiveUserById(UserId);
                if (state)
                {
                    TempData[SD.Success] = "User Successfuly DeActivated";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData[SD.Error] = "Something Went Wrong During The Operation";
                    return RedirectToPage("Index");
                }
            }
        }
    }
}
