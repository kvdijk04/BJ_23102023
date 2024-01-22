using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
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
        public async Task<IActionResult> Index(string culture, string keyword, int pageIndex = 1)
        {
            if (keyword != null) ViewBag.Keyword = keyword;

            var request = new GetListPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                LanguageId = culture

            };
            var r = await _newsService.GetPagingNews(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        public async Task<IActionResult> Detail(Guid id, string culture)
        {

            var item = await _newsService.GetNewsById(id, culture);
            //var url = Url.Action("Buy", "Products", new { id = 17 }, protocol: Request.Scheme);
            //// Returns https://localhost:5001/Products/Buy/17
            //return Content(url!);
            return View(item);
        }
        [Route("/news/popular")]
        public async Task<IActionResult> Popular(string culture, bool popular, bool promotion, string newsUrl)
        {
            ViewBag.Culture = culture;
            ViewBag.NewsUrl = newsUrl;
            popular = true;
            var news = await _newsService.GetNews(culture, popular, promotion);
            return PartialView("_PopularNews", news);
        }
    }
}
