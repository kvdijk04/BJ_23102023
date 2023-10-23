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

        public ProductController(ILogger<ProductController> logger, IProductServiceConnection productService, ICategoryServiceConnection categoryServiceConnection, ISizeServiceConnection sizeService)
        {
            _logger = logger;
            _productService = productService;
            _categoryServiceConnection = categoryServiceConnection;
            _sizeService = sizeService;
        }


        [Route("/{culture}/drinks.html")]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1)
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
            var result = await _categoryServiceConnection.GetAllUserCategories();

            return View(result);
        }
        [Route("/singleproduct", Name = "UserProductDetail")]
        public async Task<IActionResult> Detail(Guid proId)
        {
            //var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            var product = await _productService.GetUserProductById(proId);

            if (product == null)
            {
                return Redirect("/khong-tim-thay-trang.html");
            }

            return PartialView("_SingleProduct", product);
        }

        [Route("/filtercategory", Name = "UserFilterCategory")]
        public async Task<IActionResult> FilterByCategoryId(Guid catId,bool popular)
        {
            //var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            var Size = await _productService.GetAllProductByCatId(catId,popular);

            if (Size == null)
            {
                return Redirect("/khong-tim-thay-trang.html");
            }

            return PartialView("_FilterByCategory", Size);
        }
    }
}
