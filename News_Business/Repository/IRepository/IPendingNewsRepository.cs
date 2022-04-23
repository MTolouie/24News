using News_Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Business.Repository.IRepository
{
    public interface IPendingNewsRepository
    {
        public Task<bool> SendToPendingNews(PendingNewsDTO PendingDTO);
        public PendingNewsDTO GetPendingNews(int NewsId,string UserId);
        public PendingNewsDTO GetUserPendingNews(string UserId);
        public Task<bool> DeletePendingNews(int NewsId,string UserId);
    }
}
