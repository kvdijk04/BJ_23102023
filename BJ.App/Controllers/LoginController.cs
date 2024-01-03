using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVisitorCounterServiceConnection _visitorCounterServiceConnection;
        public LoginController(ILogger<LoginController> logger, IHttpContextAccessor httpContextAccessor, IVisitorCounterServiceConnection visitorCounterServiceConnection)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _visitorCounterServiceConnection = visitorCounterServiceConnection;
        }
        [Route("/{culture}/vibe")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
