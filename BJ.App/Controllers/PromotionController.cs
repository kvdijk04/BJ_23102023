using BJ.ApiConnection.Services;
using BJ.Contract.VisitorCounter;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class PromotionController : Controller
    {
        private readonly ILogger<PromotionController> _logger;
        private readonly INewsServiceConnection _promotionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVisitorCounterServiceConnection _visitorCounterServiceConnection;
        public PromotionController(ILogger<PromotionController> logger, INewsServiceConnection promotionService, IHttpContextAccessor httpContextAccessor, IVisitorCounterServiceConnection visitorCounterServiceConnection)
        {
            _logger = logger;
            _promotionService = promotionService;
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
            ViewBag.Counter = a;

            var promotion = await _promotionService.GetPromotions(culture);

            return View(promotion);
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
            var item = await _promotionService.GetNewsById(id, culture);
            return View(item);
        }

    }
}
