using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;

namespace News_Web.Pages.Admin.Users
{
    [Authorize(Roles = SD.Role_Admin)]
    public class IndexModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public IndexModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Tuple<List<ApplicationUserDTO>, int> UserDTO { get; set; }
        public async Task OnGet(int pageId = 1,string Name="",string Role="",string FromDate= "", string ToDate="")
        {
            ViewData["PageId"] = pageId;
            ViewData["Role"] = Role;
            ViewData["name"] = Name;
            ViewData["FromDate"] = FromDate;
            ViewData["ToDate"] = ToDate;
            if (!string.IsNullOrEmpty(FromDate))
            {
                FromDate = FromDate.Replace("-", "/");
            }
            if (!string.IsNullOrEmpty(ToDate))
            {
                ToDate = ToDate.Replace("-", "/");
            }
            UserDTO = await _userRepository.GetAllUsers(pageId,Name,Role,FromDate,ToDate);
        }
    }
}
