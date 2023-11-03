using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewsServiceConnection _newsService;

        public NewsController(ILogger<NewsController> logger, INewsServiceConnection newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }
        public async Task<IActionResult> Index(string culture, bool popular)
        {
            var news = await _newsService.GetAllNews(culture, popular);

            return View(news);
        }
        public async Task<IActionResult> Detail(Guid id, string culture)
        {
            ViewBag.Culture = culture;
            var item = await _newsService.GetNewsById(id, culture);
            return View(item);
        }
        [Route("/news/popular")]
        public async Task<IActionResult> Popular(string culture, bool popular, string newsUrl)
        {
            ViewBag.Culture = culture;
            ViewBag.NewsUrl = newsUrl;
            var news = await _newsService.GetAllNews(culture, popular);
            return PartialView("_PopularNews", news);
        }
    }
}
