using News_Models.DTOs;
using News_Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Business.Repository.IRepository
{
    public interface INewsRepository
    {
        public Task<Tuple<List<NewsDTO>, int>> GetAllNews(int Category ,int pageId, string Name = "", string FromDate = "", string ToDate = "");
        public Task<List<NewsForIndexViewModel>> GetNewsForSliders();
        public Task<List<NewsForIndexViewModel>> GetLatestNews();
        public Task<IEnumerable<NewsForIndexViewModel>> GetNewsByCategoryId(int Id);
        public Task<Tuple<IEnumerable<NewsForIndexViewModel>, int>> GetNewsByCategoryIdForCategoryIndex(int Id, int pageId = 1);
        public Task<List<NewsForIndexViewModel>> GetTrendingNewsForIndex();
        public Task<List<NewsForIndexViewModel>> GetLastWeekBestNews();
        public Task<Tuple<List<NewsDTO>, int>> GetAllArchivedNews(int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "");
        public Task<Tuple<List<NewsDTO>, int>> GetAllUnPublishedNews(int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "");
        public Task<Tuple<List<NewsDTO>, int>> GetAllUserUnPublishedNews(string UserId,int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "");
        public Task<Tuple<List<NewsDTO>, int>> GetAllUserNews(string UserId,int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "");
        public Task<Tuple<List<NewsDTO>, int>> GetUserArchivedNews(string UserId, int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "");
        public Task<Tuple<List<NewsDTO>, int>> GetUserPendingNews(string UserId, int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "");
        //public Task<Tuple<List<NewsDTO>, int>> GetUserUnPublishedNews(string UserId, int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "");
        public Task<NewsDTO> GetNewsById(int NewsId);
        public Task<bool> CreateNews(NewsDTO newsDTO);
        public Task<NewsDTO> UpdateNews(int NewsId, NewsDTO newsDTO);
        public Task<bool> ArchiveNewsById(int NewsId);
        public Task<bool> UnArchiveNewsById(int NewsId);
        public Task<bool> DeleteNewsById(int NewsId);
        public Task<int> GetNewsCount();
        public Task<int> GetUserNewsCount(string UserId);
        public Task<int> GetArchivedNewsCount();
        public Task<int> GetUnPublishedNewsCount();
        public int GetUserPendingNewsCount(string UserId);
        public int GetUserUnPublishedNewsCount(string UserId);
        public int GetUserNewsViewCount(string UserId);
        public Task<int> GetUserArchivedNewsCount(string UserId);
        public IEnumerable<NewsDTO> GetUserTopNewsByViewCount(string UserId);
        public Task<SingleNewsViewModel> GetNewsForView(int NewsId);
        public Task<Tuple<List<NewsForIndexViewModel>, int>> GetNewsBySearch(string q, int pageId);

    }
}
