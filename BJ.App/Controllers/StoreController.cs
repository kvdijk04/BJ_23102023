using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> _logger;
        private readonly IStoreLocationServiceConnection _storeLocationServiceConnection;
        private readonly IConfiguration _configuration;

        public StoreController(ILogger<StoreController> logger, IStoreLocationServiceConnection storeLocationServiceConnection, IConfiguration configuration)
        {
            _logger = logger;
            _storeLocationServiceConnection = storeLocationServiceConnection;
            _configuration = configuration;

        }
        public Task<IActionResult> Index(string culture)
        {

            return Task.FromResult<IActionResult>(View());
        }

        [Route("/store")]
        public async Task<JsonResult> GetStore()
        {
            var store = await _storeLocationServiceConnection.GetAllStoreLocations();

            return Json(store);
        }
    }
}