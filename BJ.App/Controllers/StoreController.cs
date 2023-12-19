using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> _logger;
        private readonly IStoreLocationServiceConnection _storeLocationServiceConnection;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVisitorCounterServiceConnection _visitorCounterServiceConnection;
        public StoreController(ILogger<StoreController> logger, IStoreLocationServiceConnection storeLocationServiceConnection, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IVisitorCounterServiceConnection visitorCounterServiceConnection)
        {
            _logger = logger;
            _storeLocationServiceConnection = storeLocationServiceConnection;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _visitorCounterServiceConnection = visitorCounterServiceConnection;

        }
        public async Task<IActionResult> Index(string culture)
        {

            var store = await _storeLocationServiceConnection.GetAllStoreLocations();

            return View(store);
        }

        public async Task<JsonResult> GetStore()
        {
            var store = await _storeLocationServiceConnection.GetAllStoreLocations();

            return Json(store);
        }

    }
}