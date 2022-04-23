using News_Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Business.Repository.IRepository
{
    public interface IUserRepository
    {
        public Task<Tuple<List<ApplicationUserDTO>, int>> GetAllUsers(int pageId, string Name = "", string Role = "",string FromDate="",string ToDate = "");
        public Task<Tuple<List<ApplicationUserDTO>, int>> GetDeActiveUsers(int pageId, string Name = "", string Role = "", string FromDate = "", string ToDate = "");
        public Task<ApplicationUserDTO> GetUserById(string UserId);
        //public Task<bool> CreateNews(NewsDTO newsDTO);
        //public Task<NewsDTO> UpdateNews(int NewsId, NewsDTO newsDTO);
        public Task<bool> DeActivateUserById(string UserId);
        public Task<bool> ActiveUserById(string UserId);
        public Task<bool> DeleteUserById(string UserId);
        public Task<int> GetUserCount();
        public Task<bool> GiveUserJournalistRole(string UserId);
        public Task<bool> GiveUserUserRole(string UserId);
        public string GetUserRoleName(string UserId);
        public int GetAllUserCount();
        public int GetJournalistCount();
        public ApplicationUserDTO GetUserByUserName(string Name);
        public  Task<bool> UpdateUserAvatar(string UserId,string FileName);
    }
}
