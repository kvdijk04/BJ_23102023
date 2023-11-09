using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class PromotionController : Controller
    {
        private readonly ILogger<PromotionController> _logger;
        private readonly INewsServiceConnection _promotionService;

        public PromotionController(ILogger<PromotionController> logger, INewsServiceConnection promotionService)
        {
            _logger = logger;
            _promotionService = promotionService;
        }
        public async Task<IActionResult> Index(string culture)
        {
            var promotion = await _promotionService.GetPromotions(culture);

            return View(promotion);
        }
        public async Task<IActionResult> Detail(Guid id, string culture)
        {
            ViewBag.Culture = culture;
            var item = await _promotionService.GetNewsById(id, culture);
            return View(item);
        }

    }
}
