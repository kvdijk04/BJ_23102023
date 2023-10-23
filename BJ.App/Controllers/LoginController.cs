using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class LoginController : Controller
    {
        [Route("/{culture}/vibe.html")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
