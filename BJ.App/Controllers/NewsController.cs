using BJ.ApiConnection.Services;
using BJ.Contract.VisitorCounter;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewsServiceConnection _newsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVisitorCounterServiceConnection _visitorCounterServiceConnection;
        public NewsController(ILogger<NewsController> logger, INewsServiceConnection newsService, IHttpContextAccessor httpContextAccessor, IVisitorCounterServiceConnection visitorCounterServiceConnection)
        {
            _logger = logger;
            _newsService = newsService;
            _httpContextAccessor = httpContextAccessor;
            _visitorCounterServiceConnection = visitorCounterServiceConnection;
        }
        public async Task<IActionResult> Index(string culture, bool popular)
        {
            string visitorId = _httpContextAccessor.HttpContext.Request.Cookies["VisitorId"];

            if (visitorId == null)
            {
                UpdateVisitorCounterDto updateVisitorCounterDto = new();
                await _visitorCounterServiceConnection.UpdateVisitorCounter(updateVisitorCounterDto);
            }
            var a = await _visitorCounterServiceConnection.GetVisitorCounter();
            ViewBag.Counter = a;

            var news = await _newsService.GetAllNews(culture, popular);

            return View(news);
        }
        public async Task<IActionResult> Detail(Guid id, string culture)
        {
            string visitorId = _httpContextAccessor.HttpContext.Request.Cookies["VisitorId"];

            if (visitorId == null)
            {
                UpdateVisitorCounterDto updateVisitorCounterDto = new();
                await _visitorCounterServiceConnection.UpdateVisitorCounter(updateVisitorCounterDto);
            }
            var a = await _visitorCounterServiceConnection.GetVisitorCounter();
            ViewBag.Counter = a;

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
