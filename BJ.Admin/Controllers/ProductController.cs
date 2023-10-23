using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BJ.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductServiceConnection _productService;
        private readonly ISizeServiceConnection _sizeService;
        private readonly ISubCategoryServiceConnection _subCategoryService;
        private readonly ICategoryServiceConnection _categoryService;

        public ProductController(ILogger<ProductController> logger, IProductServiceConnection productService, ISizeServiceConnection sizeService, ISubCategoryServiceConnection subCategoryService, ICategoryServiceConnection categoryService)
        {
            _logger = logger;
            _productService = productService;
            _sizeService = sizeService;
            _subCategoryService = subCategoryService;
            _categoryService = categoryService;
        }
        [Route("/tat-ca-san-pham.html")]
        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1)
        {
            if (keyword != null) ViewBag.Keyword = keyword;

            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var request = new GetListPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,

            };
            var r = await _productService.GetPaging(request);

            var listCat = await _categoryService.GetAllCategories();

            ViewBag.ListCat = listCat;

            //ViewData["CategoryId"] = new SelectList(listCat, "Id", "CatName");

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("/tao-moi-san-pham.html")]
        public async Task<IActionResult> Create()
        {
            var size = await _sizeService.GetAllSizes();
            var cat = await _subCategoryService.GetAllSubCategories();

            var listCat = await _categoryService.GetAllCategories();

            ViewData["CategoryId"] = new SelectList(listCat, "Id", "CatName");

            ViewBag.SizeDtos = size.ToList();
            ViewBag.SubCategoryDtos = cat.ToList();

            return View();
        }
        [Route("/cap-nhat-san-pham/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            var listCat = await _categoryService.GetAllCategories();
            var subCat = await _subCategoryService.GetAllSubCategories();

            var size = await _sizeService.GetAllSizes();

            ViewData["CategoryId"] = new SelectList(listCat, "Id", "CatName");
            ViewBag.SizeDtos = size.ToList();
            ViewBag.SubCategoryDtos = subCat.ToList();
            ViewBag.ProductId = id;
            var result = await _productService.GetProductById(id);
            var updateResult = new UpdateProductAdminView()
            {
                UpdateProductDto = new()
                {
                    Active = result.Active,
                    BestSeller = result.BestSeller,
                    Description = result.Description,
                    HomeTag = result.HomeTag,
                    ImagePathCup = result.ImagePathCup,
                    ImagePathHero = result.ImagePathHero,
                    ImagePathIngredients = result.ImagePathIngredients,
                    MetaDesc = result.MetaDesc,
                    MetaKey = result.MetaKey,
                    ProductName = result.ProductName,
                    ShortDesc = result.ShortDesc,
                    Tags = result.Tags,
                    CategoryId = result.CategoryId,

                },

                SizeSpecificProductDto = result.SizeSpecificProducts.ToList(),
                SubCategorySpecificProductDtos = result.SubCategorySpecificProductDtos.ToList(),
            };
            return View(updateResult);
        }
        [Route("/cap-nhat-san-pham/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [FromForm] UpdateProductAdminView updateProductAdminView)
        {

            var listCat = await _categoryService.GetAllCategories();

            ViewData["CategoryId"] = new SelectList(listCat, "Id", "CatName");

            var result = await _productService.GetProductById(id);

            if (result == null)
            {
                return Redirect("/tat-ca-san-pham.html");
            }
            if (updateProductAdminView.ImageIngredients == null) { updateProductAdminView.UpdateProductDto.ImagePathIngredients = result.ImagePathIngredients; }
            if (updateProductAdminView.ImageHero == null) { updateProductAdminView.UpdateProductDto.ImagePathHero = result.ImagePathHero; }

            if (updateProductAdminView.ImageCup == null) { updateProductAdminView.UpdateProductDto.ImagePathCup = result.ImagePathCup; }

            await _productService.UpdateProduct(id, updateProductAdminView);

            return Redirect("/cap-nhat-san-pham/" + id);
        }
        [Route("chi-tiet-san-pham/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var r = await _productService.GetProductById(id);

            return View(r);
        }


        [HttpPost]
        [Route("/tao-moi-san-pham.html")]

        public async Task<IActionResult> Create(CreateProductAdminView createProductAdminView)
        {
            var size = await _sizeService.GetAllSizes();
            var cat = await _subCategoryService.GetAllSubCategories();

            var listCat = await _categoryService.GetAllCategories();

            ViewData["CategoryId"] = new SelectList(listCat, "Id", "CatName");

            ViewBag.SizeDtos = size.ToList();
            ViewBag.SubCategoryDtos = cat.ToList();
            await _productService.CreateProduct(createProductAdminView);

            return View();
        }
    }
}
