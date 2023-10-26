using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class NewsController : Controller
    {
        [Route("/{culture}/news.html")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
