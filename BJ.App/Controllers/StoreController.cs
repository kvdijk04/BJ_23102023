using BJ.ApiConnection.Services;
using BJ.Contract.VisitorCounter;
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
            string visitorId = _httpContextAccessor.HttpContext.Request.Cookies["VisitorId"];

            if (visitorId == null)
            {
                UpdateVisitorCounterDto updateVisitorCounterDto = new();
                await _visitorCounterServiceConnection.UpdateVisitorCounter(updateVisitorCounterDto);
            }
            var a = await _visitorCounterServiceConnection.GetVisitorCounter();
            var store = await _storeLocationServiceConnection.GetAllStoreLocations();

            ViewBag.Day = a.DayCount;
            ViewBag.Month = a.MonthCount;
            ViewBag.Year = a.YearCount;
            return View(store);
        }

        [Route("/store")]
        public async Task<JsonResult> GetStore()
        {
            var store = await _storeLocationServiceConnection.GetAllStoreLocations();

            return Json(store);
        }
        
    }
}