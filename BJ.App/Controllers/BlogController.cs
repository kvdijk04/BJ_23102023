using BJ.ApiConnection.Services;
using BJ.Contract.VisitorCounter;
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

            var blog = await _blogService.GetAllBlogs(culture, popular);

            return View(blog);
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
            var item = await _blogService.GetBlogById(id, culture);
            return View(item);
        }
        [Route("/wellbeing/popular")]
        public async Task<IActionResult> Popular(string culture, bool popular, string wellbeingUrl)
        {
            ViewBag.Culture = culture;
            ViewBag.WellbeingUrl = wellbeingUrl;

            var blog = await _blogService.GetAllBlogs(culture, popular);
            return PartialView("_PopularPost", blog);
        }
    }
}
