using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class BlogController : Controller
    {
        [Route("/{culture}/news.html")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
    }
}
