using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductServiceConnection _productService;
        private readonly ICategoryServiceConnection _categoryServiceConnection;
        private readonly ISizeServiceConnection _sizeService;
        private readonly INotyfService _notyfService;
        public ProductController(ILogger<ProductController> logger, IProductServiceConnection productService, ICategoryServiceConnection categoryServiceConnection, ISizeServiceConnection sizeService, INotyfService notyf)
        {
            _logger = logger;
            _productService = productService;
            _categoryServiceConnection = categoryServiceConnection;
            _sizeService = sizeService;
            _notyfService = notyf;
        }

        public async Task<IActionResult> Index(string culture, string keyword, int pageIndex = 1)
        {

            //var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            //var request = new GetListPagingRequest()
            //{
            //    Keyword = keyword,
            //    PageIndex = pageIndex,
            //};

            //var product = await _productService.GetPaging(request);
            TempData["Cul"] = culture;
            var result = await _productService.GetAllUserProduct(culture);

            return View(result);
        }
        public async Task<IActionResult> Detail(Guid proId, string culture,string alias)
        {
            //var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            var product = await _productService.GetUserProductById(proId, culture);

            if (product == null)
            {
                return Redirect("/khong-tim-thay-trang.html");
            }

            return View(product);
        }

        [Route("/filtercategory", Name = "UserFilterCategory")]
        public async Task<IActionResult> FilterByCategoryId(Guid catId, string culture)
        {
            //var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            var Size = await _productService.GetAllProductByCatId(catId, culture);

            if (Size == null)
            {
                return Redirect("/khong-tim-thay-trang.html");
            }

            return PartialView("_FilterByCategory", Size);
        }
        public async Task<IActionResult> Buy(string culture)
        {
            //< a href =/ " + a+" / xem - chi - tiet > Xem chi tiết</ a >
            var a = TempData["Cul"];
            //_notyfService.Information("<p>Đặt hàng qua app</p><a href=http://bit.ly/1yqA2a2 target=_blank><img src=../../images/ios.png width=80></a><a style=margin-left:10px href=http://bit.ly/1yqA2a2 target=_blank><img src=../../images/ggplay.png width=80></a>",20);
            _notyfService.Information("Vui lòng đặt qua app<br/>Boost Juice Việt Nam hoặc gọi hotline");
            return RedirectToAction("Index", "Product");
        }
    }
}
