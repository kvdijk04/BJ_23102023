using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IBlogServiceConnection _blogService;

        public BlogController(ILogger<BlogController> logger, IBlogServiceConnection blogService)
        {
            _logger = logger;
            _blogService = blogService;
        }
        public async Task<IActionResult> Index(string culture, bool popular)
        {
            var blog = await _blogService.GetAllBlogs(culture, popular);

            return View(blog);
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

            var blog = await _blogService.GetAllBlogs(culture, popular);
            return PartialView("_PopularPost", blog);
        }
    }
}
