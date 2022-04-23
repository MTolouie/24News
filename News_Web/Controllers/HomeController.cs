using Microsoft.AspNetCore.Mvc;
using News_Business.Repository.IRepository;
using News_Web.Models;
using System.Diagnostics;

namespace News_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsRepository _newsRepository;
        //private readonly ICategoryRepository _categoryRepository;

        public HomeController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

       public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Error404()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}