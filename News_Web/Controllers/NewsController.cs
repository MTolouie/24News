using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News_Business.Repository.IRepository;
using News_Common;
using News_Models.DTOs;

namespace News_Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;

        public NewsController(INewsRepository newsRepository, ICategoryRepository categoryRepository, IUserRepository userRepository, ICommentRepository commentRepository)
        {
            _newsRepository = newsRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> SingleNews(int NewsId)
        {
            
            var news = await _newsRepository.GetNewsForView(NewsId);
            if (news == null)
            {
                return NotFound();
            }
            ViewBag.Category = await _categoryRepository.GetCategoryTitleById(news.News.CatId);
            return View(news);
        }

        //[Authorize(Roles = News_Common.SD.Role_User)]
        public async Task<IActionResult> NewsComments(int NewsId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var user = _userRepository.GetUserByUserName(username);
                ViewData["UserAvatar"] = user.Avatar;
                ViewData["Username"] = user.Name;
                ViewData["NewsId"] = NewsId;
                ViewData["UserId"] = user.Id;
            }
            return View( await _commentRepository.GetCommentsByNewsId(NewsId));
        }

        [HttpPost]
        [Authorize(Roles = News_Common.SD.Role_User)]
        public async Task<IActionResult> NewsComments(int NewsId,string UserId,string CommentText)
        {
            if (!ModelState.IsValid)
            {
                TempData[SD.Error] = "Not Valid";
                return Redirect("/News/NewsComments?NewsId=" + NewsId);
            }
            if (string.IsNullOrEmpty(CommentText))
            {
                TempData[SD.Error] = "Not Valid";
                return Redirect("/News/NewsComments?NewsId=" + NewsId);
            }
            var username = User.Identity.Name;
            var user =  _userRepository.GetUserByUserName(username);

            ViewData["UserAvatar"] = user.Avatar;
            ViewData["Username"] = user.Name;
            ViewData["NewsId"] = NewsId;
            ViewData["UserId"] = user.Id;

            if (user.Id != UserId)
            {
                TempData[SD.Error] = "Something Went Wrong";
                return Redirect("News/NewsComments?NewsId=" + NewsId);
            }


            CommentDTO Comment = new CommentDTO()
            {
                CommentText = CommentText,
                CreateDate = DateTime.Now,
                NewsId = NewsId,
                UserId = UserId
            };


            var state = await _commentRepository.CreateComments(Comment);
            if (state == true)
            {
                TempData[SD.Success] = "Comment Created Succesfuly";
            }
            else
            {
                TempData[SD.Error] = "Comment Not Valid";
            }

            return Redirect("/News/NewsComments?NewsId=" + NewsId);

        }


        public async Task<IActionResult> Search(string q,int PageId = 1)
        {
            var newses = await _newsRepository.GetNewsBySearch(q,PageId);
            ViewData["q"] = q;
            ViewData["PageId"] = PageId;
            return View(newses);
        }
    }
}
