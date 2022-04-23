using AutoMapper;
using Microsoft.EntityFrameworkCore;
using News_Business.Repository.IRepository;
using News_DataLayer.Data;
using News_DataLayer.Models;
using News_Models.DTOs;
using News_Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Business.Repository
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public NewsRepository(ApplicationDbContext db, IMapper mapper,ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _db = db;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> ArchiveNewsById(int NewsId)
        {
            try
            {
                var news = await _db.News.FindAsync(NewsId);
                if (news == null)
                {
                    return false;
                }
                news.IsArchived = true;
                _db.News.Update(news);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> CreateNews(NewsDTO newsDTO)
        {
            try
            {
                var news = _mapper.Map<NewsDTO, News>(newsDTO);
                news.IsArchived = false;
                news.CreateDate = DateTime.Now;
                news.Visit = 0;
                _db.News.Add(news);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteNewsById(int NewsId)
        {
            try
            {
                var news = await _db.News.FindAsync(NewsId);
                if (news == null)
                {
                    return false;
                }
                _db.News.Remove(news);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Tuple<List<NewsDTO>, int>> GetAllArchivedNews(int category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "")
        {


            try
            {
                IQueryable<News> news = _db.News.Where(n => n.IsArchived && n.IsPublished && !n.IsPending);

                int take = 5;
                int skip = (pageId - 1) * take;

                if (!string.IsNullOrEmpty(Title))
                {
                    news = news.Where(u => u.NewsTitle.Contains(Title));

                }
                if (category != 0)
                {
                    news = news.Where(n => n.CatId == category);
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    DateTime fromdate = DateTime.ParseExact(FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate >= fromdate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    DateTime toDate = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate <= toDate);
                }

                var newsDTO = _mapper.Map<IQueryable<News>, IEnumerable<NewsDTO>>(news);

                var pageCount = newsDTO.Count() / take;
                if (pageCount % 2 != 0)
                {
                    pageCount++;
                }
                var query = newsDTO.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();

                return Tuple.Create(query, pageCount);
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<Tuple<List<NewsDTO>, int>> GetAllNews(int category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "")
        {
            try
            {
                IQueryable<News> news = _db.News.Where(n => !n.IsArchived && n.IsPublished && !n.IsPending);

                int take = 5;
                int skip = (pageId - 1) * take;

                if (!string.IsNullOrEmpty(Title))
                {
                    news = news.Where(u => u.NewsTitle.Contains(Title));

                }
                if (category != 0)
                {
                    news = news.Where(n => n.CatId == category);
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    DateTime fromdate = DateTime.ParseExact(FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate >= fromdate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    DateTime toDate = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate <= toDate);
                }

                var newsDTO = _mapper.Map<IQueryable<News>, IEnumerable<NewsDTO>>(news);

                var pageCount = newsDTO.Count() / take;
                if (pageCount % 2 != 0)
                {
                    pageCount++;
                }
                var query = newsDTO.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();

                return Tuple.Create(query, pageCount);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Tuple<List<NewsDTO>, int>> GetAllUnPublishedNews(int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "")
        {
            try
            {
                IQueryable<News> news = _db.News.Where(n => !n.IsArchived && !n.IsPublished && !n.IsPending);

                int take = 5;
                int skip = (pageId - 1) * take;

                if (!string.IsNullOrEmpty(Title))
                {
                    news = news.Where(u => u.NewsTitle.Contains(Title));

                }
                if (Category != 0)
                {
                    news = news.Where(n => n.CatId == Category);
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    DateTime fromdate = DateTime.ParseExact(FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate >= fromdate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    DateTime toDate = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate <= toDate);
                }

                var newsDTO = _mapper.Map<IQueryable<News>, IEnumerable<NewsDTO>>(news);

                var pageCount = newsDTO.Count() / take;
                if (pageCount % 2 != 0)
                {
                    pageCount++;
                }
                var query = newsDTO.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();

                return Tuple.Create(query, pageCount);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Tuple<List<NewsDTO>, int>> GetAllUserNews(string UserId, int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "")
        {
            try
            {
                IQueryable<News> news = _db.News.Where(n => n.UserId == UserId && !n.IsArchived && n.IsPublished && !n.IsPending);

                int take = 5;
                int skip = (pageId - 1) * take;

                if (!string.IsNullOrEmpty(Title))
                {
                    news = news.Where(u => u.NewsTitle.Contains(Title));

                }
                if (Category != 0)
                {
                    news = news.Where(n => n.CatId == Category);
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    DateTime fromdate = DateTime.ParseExact(FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate >= fromdate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    DateTime toDate = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate <= toDate);
                }

                var newsDTO = _mapper.Map<IQueryable<News>, IEnumerable<NewsDTO>>(news);

                var pageCount = newsDTO.Count() / take;
                if (pageCount % 2 != 0)
                {
                    pageCount++;
                }
                var query = newsDTO.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();

                return Tuple.Create(query, pageCount);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Tuple<List<NewsDTO>, int>> GetAllUserUnPublishedNews(string UserId, int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "")
        {
            try
            {
                IQueryable<News> news = _db.News.Where(n => !n.IsArchived && !n.IsPublished && !n.IsPending && n.UserId == UserId);

                int take = 5;
                int skip = (pageId - 1) * take;

                if (!string.IsNullOrEmpty(Title))
                {
                    news = news.Where(u => u.NewsTitle.Contains(Title));

                }
                if (Category != 0)
                {
                    news = news.Where(n => n.CatId == Category);
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    DateTime fromdate = DateTime.ParseExact(FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate >= fromdate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    DateTime toDate = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate <= toDate);
                }

                var newsDTO = _mapper.Map<IQueryable<News>, IEnumerable<NewsDTO>>(news);

                var pageCount = newsDTO.Count() / take;
                if (pageCount % 2 != 0)
                {
                    pageCount++;
                }
                var query = newsDTO.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();

                return Tuple.Create(query, pageCount);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<int> GetArchivedNewsCount()
        {
            try
            {
                var newsCount = await _db.News.Where(n => n.IsArchived && !n.IsPending).CountAsync();
                return newsCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<List<NewsForIndexViewModel>> GetLastWeekBestNews()
        {
            try
            {
                List<NewsForIndexViewModel> newsForIndexViewModels = new List<NewsForIndexViewModel>();
                var news = await _db.News
                    .Where(n => !n.IsArchived && !n.IsPending && n.IsPublished && n.CreateDate <= DateTime.Now.AddDays(-7) && n.CreateDate >= DateTime.Now.AddDays(-14) && n.Visit > _db.Users.Count())
                    .OrderByDescending(n => n.Visit)
                    .ToListAsync();
                var newsDto = _mapper.Map<List<News>, List<NewsDTO>>(news);
                foreach (var item in newsDto)
                {
                    NewsForIndexViewModel ViewModel = new NewsForIndexViewModel()
                    {
                        NewsId = item.NewsId,
                        CreateDate = item.CreateDate,
                        Images = item.Images,
                        NewsTitle = item.NewsTitle,
                        Author = _db.ApplicationUser.SingleOrDefault(u => u.Id == item.UserId).Name,
                        Visit = item.Visit,
                        CatTitle = await _categoryRepository.GetCategoryTitleById(item.CatId),
                        ShortDesc = item.ShortDesc
                    };
                    newsForIndexViewModels.Add(ViewModel);
                }
                return newsForIndexViewModels;


            }
            catch (Exception e)
            {

                return null;
            }
        }

        public async Task<List<NewsForIndexViewModel>> GetLatestNews()
        {
            try
            {
                List<NewsForIndexViewModel> newsForIndexViewModels = new List<NewsForIndexViewModel>();
                var news = await _db.News.Where(n=>n.IsPublished && !n.IsArchived && !n.IsPending).OrderByDescending(c => c.CreateDate).Take(8).ToListAsync();
                var newsDTO = _mapper.Map<List<News>,List<NewsDTO>>(news);

                foreach (var item in newsDTO)
                {
                    NewsForIndexViewModel ViewModel = new NewsForIndexViewModel()
                    {
                        NewsId = item.NewsId,
                        CreateDate = item.CreateDate,
                        Images = item.Images,
                        NewsTitle = item.NewsTitle,
                        Author = _db.ApplicationUser.SingleOrDefault(u => u.Id == item.UserId).Name,
                        ShortDesc = item.ShortDesc,
                        CatTitle = await _categoryRepository.GetCategoryTitleById(item.CatId),
                        Visit = item.Visit
                    };
                    newsForIndexViewModels.Add(ViewModel);
                }
                return newsForIndexViewModels;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<NewsForIndexViewModel>> GetNewsByCategoryId(int Id)
        {
            try
            {
                List<NewsForIndexViewModel> newsForIndexViewModels = new List<NewsForIndexViewModel>();
                var news = await _db.News.Where(n => n.CatId == Id && n.IsPublished && !n.IsPending && !n.IsArchived && n.CreateDate >= DateTime.Now.AddDays(-7)).ToListAsync();
                if (news == null)
                {
                    return null;
                }
                var newsDTO = _mapper.Map<List<News>, List<NewsDTO>>(news);

               

                foreach (var item in newsDTO)
                {
                    NewsForIndexViewModel ViewModel = new NewsForIndexViewModel()
                    {
                        NewsId = item.NewsId,
                        CreateDate = item.CreateDate,
                        Images = item.Images,
                        NewsTitle = item.NewsTitle,
                        CatTitle = await _categoryRepository.GetCategoryTitleById(item.CatId),
                        ShortDesc = item.ShortDesc,
                    };
                    newsForIndexViewModels.Add(ViewModel);
                }
                return newsForIndexViewModels;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Tuple<IEnumerable<NewsForIndexViewModel>, int>> GetNewsByCategoryIdForCategoryIndex(int Id, int pageId = 1)
        {
            
                try
                {
                    List<NewsForIndexViewModel> newsForIndexViewModels = new List<NewsForIndexViewModel>();
                    var news = await _db.News.Where(n => n.CatId == Id && n.IsPublished && !n.IsPending && !n.IsArchived).ToListAsync();
                    if (news == null)
                    {
                        return null;
                    }
                    var newsDTO = _mapper.Map<List<News>, List<NewsDTO>>(news);

                    int take = 6;
                    int skip = (pageId - 1) * take;

                    foreach (var item in newsDTO)
                    {
                        NewsForIndexViewModel ViewModel = new NewsForIndexViewModel()
                        {
                            NewsId = item.NewsId,
                            CreateDate = item.CreateDate,
                            Images = item.Images,
                            NewsTitle = item.NewsTitle,
                            CatTitle = await _categoryRepository.GetCategoryTitleById(item.CatId),
                            ShortDesc = item.ShortDesc,
                        };
                        newsForIndexViewModels.Add(ViewModel);
                    }

                    var pageCount = newsDTO.Count() / take;
                    if (pageCount % 2 != 0)
                    {
                        pageCount++;
                    }
                    var query = newsForIndexViewModels.OrderByDescending(c=>c.CreateDate).Skip(skip).Take(take);
                    return Tuple.Create(query, pageCount);

                }
                catch (Exception)
                {
                    return null;
                }
            
        }

        public async Task<NewsDTO> GetNewsById(int NewsId)
        {
            try
            {
                var news = await _db.News.FindAsync(NewsId);
                if (news == null)
                {
                    return null;
                }
                var newsDTO = _mapper.Map<News, NewsDTO>(news);
                return newsDTO;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Tuple<List<NewsForIndexViewModel>, int>> GetNewsBySearch(string q,int pageId)
        {
            try
            {
                List<NewsForIndexViewModel> newsForIndexViewModels = new List<NewsForIndexViewModel>();
                IQueryable<News> news = _db.News.Where(n => !n.IsArchived && n.IsPublished && !n.IsPending);

                int take = 6;
                int skip = (pageId - 1) * take;

                if (!string.IsNullOrEmpty(q))
                {
                    news = news.Where(u => u.NewsTitle.Contains(q) || u.ShortDesc.Contains(q));

                }
               

                var newsDTO = _mapper.Map<IQueryable<News>, IEnumerable<NewsDTO>>(news);

                foreach (var item in newsDTO)
                {
                    NewsForIndexViewModel ViewModel = new NewsForIndexViewModel()
                    {
                        NewsId = item.NewsId,
                        CreateDate = item.CreateDate,
                        Images = item.Images,
                        NewsTitle = item.NewsTitle,
                        CatTitle = await _categoryRepository.GetCategoryTitleById(item.CatId),
                        ShortDesc = item.ShortDesc,
                    };
                    newsForIndexViewModels.Add(ViewModel);
                }


                var pageCount = newsDTO.Count() / take;
                if (pageCount % 2 != 0)
                {
                    pageCount++;
                }
                var query = newsForIndexViewModels.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();

                return Tuple.Create(query, pageCount);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<int> GetNewsCount()
        {
            try
            {
                var newsCount = await _db.News.Where(n => n.IsPublished).CountAsync();
                return newsCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<List<NewsForIndexViewModel>> GetNewsForSliders()
        {
            try
            {
                List<NewsForIndexViewModel> newsForIndexViewModels = new List<NewsForIndexViewModel>();
                var news = await _db.News
                    .Where(n => !n.IsArchived && !n.IsPending && n.IsPublished)
                    .OrderByDescending(n => n.CreateDate)
                    .ToListAsync();
                var newsDto = _mapper.Map<List<News>, List<NewsDTO>>(news);
                foreach (var item in newsDto)
                {
                    NewsForIndexViewModel ViewModel = new NewsForIndexViewModel()
                    {
                        NewsId = item.NewsId,
                        CreateDate = item.CreateDate,
                        Images = item.Images,
                        NewsTitle = item.NewsTitle,
                        ShortDesc = item.ShortDesc,
                        Visit = item.Visit,
                    };
                    newsForIndexViewModels.Add(ViewModel);
                }
                return newsForIndexViewModels;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<SingleNewsViewModel> GetNewsForView(int NewsId)
        {
            try
            {
                var news = await GetNewsById(NewsId);
                if (news == null)
                {
                    return null;
                }
                var journalist = await _userRepository.GetUserById(news.UserId);
                if (journalist == null)
                {
                    return null;
                }
                SingleNewsViewModel ViewModel = new SingleNewsViewModel() { 
                Journalist = journalist,
                News = news
                };
                return ViewModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<NewsForIndexViewModel>> GetTrendingNewsForIndex()
        {
            try
            {
                List<NewsForIndexViewModel> newsForIndexViewModels = new List<NewsForIndexViewModel>();
                var news = await _db.News
                    .Where(n => !n.IsArchived && !n.IsPending && n.IsPublished && n.CreateDate >= DateTime.Now.AddDays(-2) && n.Visit > _db.Users.Count())
                    .OrderByDescending(n => n.Visit)
                    .ToListAsync();
                var newsDto = _mapper.Map<List<News>, List<NewsDTO>>(news);
                foreach (var item in newsDto)
                {
                    NewsForIndexViewModel ViewModel = new NewsForIndexViewModel()
                    {
                        NewsId = item.NewsId,
                        CreateDate = item.CreateDate,
                        Images = item.Images,
                        NewsTitle = item.NewsTitle,
                        Author = _db.ApplicationUser.SingleOrDefault(u => u.Id == item.UserId).Name,
                        ShortDesc = item.ShortDesc,
                        CatTitle = await _categoryRepository.GetCategoryTitleById(item.CatId),
                        Visit = item.Visit
                    };
                    newsForIndexViewModels.Add(ViewModel);
                }
                return newsForIndexViewModels;


            }
            catch (Exception e)
            {

                return null;
            }

        }

        public async Task<int> GetUnPublishedNewsCount()
        {
            try
            {
                var newsCount = await _db.News.Where(n => !n.IsPublished && !n.IsPending && !n.IsArchived).CountAsync();
                return newsCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<Tuple<List<NewsDTO>, int>> GetUserArchivedNews(string UserId, int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "")
        {
            try
            {
                IQueryable<News> news = _db.News.Where(n => n.UserId == UserId && n.IsArchived && n.IsPublished && !n.IsPending);

                int take = 5;
                int skip = (pageId - 1) * take;

                if (!string.IsNullOrEmpty(Title))
                {
                    news = news.Where(u => u.NewsTitle.Contains(Title));

                }
                if (Category != 0)
                {
                    news = news.Where(n => n.CatId == Category);
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    DateTime fromdate = DateTime.ParseExact(FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate >= fromdate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    DateTime toDate = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate <= toDate);
                }

                var newsDTO = _mapper.Map<IQueryable<News>, IEnumerable<NewsDTO>>(news);

                var pageCount = newsDTO.Count() / take;
                if (pageCount % 2 != 0)
                {
                    pageCount++;
                }
                var query = newsDTO.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();

                return Tuple.Create(query, pageCount);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<int> GetUserArchivedNewsCount(string UserId)
        {
            try
            {
                var newsCount = _db.News.Where(n => !n.IsPublished && !n.IsPending && n.IsArchived && n.UserId == UserId).Count();
                return newsCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<int> GetUserNewsCount(string UserId)
        {
            try
            {
                var newsCount = _db.News.Where(n => n.IsPublished && !n.IsPending && !n.IsArchived && n.UserId == UserId).Count();
                return newsCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int GetUserNewsViewCount(string UserId)
        {
            try
            {
                int ViewCount = 0;
                var allNews = _db.News.Where(n => n.UserId == UserId).ToList();
                //allNews.ForEach(n => ViewCount = n.Visit + ViewCount);
                foreach (var item in allNews)
                {
                    ViewCount = item.Visit + ViewCount;
                }
                return ViewCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<Tuple<List<NewsDTO>, int>> GetUserPendingNews(string UserId, int Category, int pageId = 1, string Title = "", string FromDate = "", string ToDate = "")
        {
            try
            {
                IQueryable<News> news = _db.News.Where(n => !n.IsArchived && !n.IsPublished && n.IsPending && n.UserId == UserId);

                int take = 5;
                int skip = (pageId - 1) * take;

                if (!string.IsNullOrEmpty(Title))
                {
                    news = news.Where(u => u.NewsTitle.Contains(Title));

                }
                if (Category != 0)
                {
                    news = news.Where(n => n.CatId == Category);
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    DateTime fromdate = DateTime.ParseExact(FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate >= fromdate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    DateTime toDate = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    news = news.Where(u => u.CreateDate <= toDate);
                }

                var newsDTO = _mapper.Map<IQueryable<News>, IEnumerable<NewsDTO>>(news);

                var pageCount = newsDTO.Count() / take;
                if (pageCount % 2 != 0)
                {
                    pageCount++;
                }
                var query = newsDTO.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();

                return Tuple.Create(query, pageCount);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int GetUserPendingNewsCount(string UserId)
        {
            try
            {
                var newsCount = _db.News.Where(n => !n.IsPublished && n.IsPending && !n.IsArchived && n.UserId == UserId).Count();
                return newsCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public IEnumerable<NewsDTO> GetUserTopNewsByViewCount(string UserId)
        {
            try
            {
                var wantedDate = DateTime.Today.AddYears(-1);
                var news = _db.News.Where(n => n.UserId == UserId && n.CreateDate >= wantedDate).OrderByDescending(n => n.Visit).Take(6).ToList();
                return _mapper.Map<IEnumerable<News>, IEnumerable<NewsDTO>>(news);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int GetUserUnPublishedNewsCount(string UserId)
        {
            try
            {
                var newsCount = _db.News.Where(n => !n.IsPublished && !n.IsPending && !n.IsArchived && n.UserId == UserId).Count();
                return newsCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<bool> UnArchiveNewsById(int NewsId)
        {
            try
            {
                var news = await _db.News.FindAsync(NewsId);
                if (news == null)
                {
                    return false;
                }
                news.IsArchived = false;
                _db.News.Update(news);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<NewsDTO> UpdateNews(int NewsId, NewsDTO newsDTO)
        {
            try
            {
                if (NewsId == newsDTO.NewsId)
                {
                    var NewsFromDb = await _db.News.SingleOrDefaultAsync(c => c.NewsId == NewsId);
                    var News = _mapper.Map<NewsDTO, News>(newsDTO, NewsFromDb);
                    var updatedNews = _db.News.Update(News);
                    await _db.SaveChangesAsync();
                    return _mapper.Map<News, NewsDTO>(updatedNews.Entity);

                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


    }
}
