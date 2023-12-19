using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IBlogServiceConnection _blogService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVisitorCounterServiceConnection _visitorCounterServiceConnection;
        public BlogController(ILogger<BlogController> logger, IBlogServiceConnection blogService, IHttpContextAccessor httpContextAccessor, IVisitorCounterServiceConnection visitorCounterServiceConnection)
        {
            _logger = logger;
            _blogService = blogService;
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
            var r = await _blogService.GetPagingUser(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }

        public async Task<IActionResult> Detail(Guid id, string culture)
        {

            ViewBag.Culture = culture;
            var item = await _blogService.GetBlogById(id, culture);
            return View(item);
        }
        [Route("/wellbeing/popular")]
        public async Task<IActionResult> Popular(string culture, bool popular, string wellbeingUrl)
        {
            ViewBag.Culture = culture;
            ViewBag.WellbeingUrl = wellbeingUrl;

            var blog = await _blogService.GetBlogsPopular(culture, popular);
            return PartialView("_PopularPost", blog);
        }
    }
}
