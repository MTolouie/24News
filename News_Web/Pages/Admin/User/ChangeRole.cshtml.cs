using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;

namespace News_Web.Pages.Admin.User
{
    [Authorize(Roles = SD.Role_Admin)]
    public class ChangeRoleModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public ChangeRoleModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ApplicationUserDTO UserDTO { get; set; }

        public async Task OnGet(string UserId)
        {
            UserDTO = await _userRepository.GetUserById(UserId);
        }
        public async Task<IActionResult> OnGetChangeRoleHandler(string UserId)
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

            var userRole =  _userRepository.GetUserRoleName(UserId);

            if (userRole == SD.Role_User)
            {
                var state = await _userRepository.GiveUserJournalistRole(UserId);
                if (state)
                {
                    TempData[SD.Success] = $"{user.Name} Is Now A Journalist";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData[SD.Error] = "Something Went Wrong During Change Operation";
                    return RedirectToPage("Index");

                }
            }
            if (userRole == SD.Role_Journalist)
            {
                var state = await _userRepository.GiveUserUserRole(UserId);
                if (state)
                {
                    TempData[SD.Success] = $"{user.Name} Is Now A Normal User";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData[SD.Error] = "Something Went Wrong During Change Operation";
                    return RedirectToPage("Index");

                }
            }

            TempData[SD.Error] = "You Cant Change Your Role";
            return RedirectToPage("Index");
        }
    }
}
