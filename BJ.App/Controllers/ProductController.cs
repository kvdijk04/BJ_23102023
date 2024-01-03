using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductServiceConnection _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVisitorCounterServiceConnection _visitorCounterServiceConnection;
        public ProductController(ILogger<ProductController> logger, IProductServiceConnection productService, IHttpContextAccessor httpContextAccessor, IVisitorCounterServiceConnection visitorCounterServiceConnection)
        {
            _logger = logger;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _visitorCounterServiceConnection = visitorCounterServiceConnection;
        }
        public async Task<IActionResult> Index(string culture)
        {
            TempData["Cul"] = culture;

            var result = await _productService.GetAllUserProduct(culture);

            return View(result);
        }

        public async Task<IActionResult> Detail(Guid proId, string culture, string alias)
        {

            var product = await _productService.GetUserProductById(proId, culture);

            if (product == null)
            {
                return Redirect("/khong-tim-thay-trang.html");
            }

            return View(product);
        }

        [Route("/filtercategory", Name = "UserFilterCategory")]
        public async Task<IActionResult> FilterByCategoryId(Guid catId, string culture,string order, string aliasCat)
        {

            var rs = await _productService.GetAllProductByCatId(culture, catId);

            if (rs == null)
            {
                return Redirect("/khong-tim-thay-trang.html");
            }
            ViewBag.CatName = rs.Select(x => x.CatName).First();
            ViewBag.Order = order;
            ViewBag.AliasCat = aliasCat;
            return PartialView("_FilterByCategory", rs);
        }

        //public async Task<IActionResult> Buy(string culture)
        //{
        //    //< a href =/ " + a+" / xem - chi - tiet > Xem chi tiết</ a >
        //    var a = TempData["Cul"];
        //    //_notyfService.Information("<p>Đặt hàng qua app</p><a href=http://bit.ly/1yqA2a2 target=_blank><img src=../../images/ios.png width=80></a><a style=margin-left:10px href=http://bit.ly/1yqA2a2 target=_blank><img src=../../images/ggplay.png width=80></a>",20);
        //    _notyfService.Information("Vui lòng đặt qua app<br/>Boost Juice Việt Nam hoặc gọi hotline");
        //    return RedirectToAction("Index", "Product");
        //}
    }
}
