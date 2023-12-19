using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
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
        public async Task<IActionResult> Index(string culture, string keyword, int pageIndex = 1)
        {
            if (keyword != null) ViewBag.Keyword = keyword;

            var request = new GetListPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                LanguageId = culture

            };
            var r = await _promotionService.GetPagingPromotion(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }

        public async Task<IActionResult> Detail(Guid id, string culture)
        {
            ViewBag.Culture = culture;
            var item = await _promotionService.GetNewsById(id, culture);
            return View(item);
        }

    }
}
