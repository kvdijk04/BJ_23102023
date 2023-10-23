using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.Category;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryServiceConnection _categoryServiceConnection;
        public CategoryController(ILogger<CategoryController> logger, ICategoryServiceConnection categoryServiceConnection)
        {
            _logger = logger;
            _categoryServiceConnection = categoryServiceConnection;
        }
        [Route("/tat-ca-loai.html")]
        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1)
        {
            if (keyword != null) ViewBag.Keyword = keyword;

            //var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            var request = new GetListPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,

            };
            var r = await _categoryServiceConnection.GetPagingCategory(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("chi-tiet-loai/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var r = await _categoryServiceConnection.GetCategoryById(id);
            return View(r);
        }
        [Route("/tao-moi-loai.html")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Route("/tao-moi-loai.html")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCategoryDto createCategoryDto)
        {
            var a = await _categoryServiceConnection.CreateCategory(createCategoryDto);

            return Redirect("/tao-moi-loai.html");
        }
        [Route("/cap-nhat-loai/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var item = await _categoryServiceConnection.GetCategoryById(id);
            UpdateCategoryDto updateCategoryDto = new()
            {
                Active = item.Active,
                Alias = item.Alias,
                CatName = item.CatName,
                DateUpdated = item.DateUpdated,
                Description = item.Description,
                ImagePath = item.ImagePath,
                MetaDesc = item.MetaDesc,
                MetaKey = item.MetaKey,
            };
            ViewBag.CatId = id;

            return View(updateCategoryDto);
        }
        [Route("/cap-nhat-loai/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [FromForm] UpdateCategoryDto updateCategoryDto)
        {
            var result = await _categoryServiceConnection.GetCategoryById(id);

            if (updateCategoryDto.Image == null) updateCategoryDto.ImagePath = result.ImagePath;

            var a = await _categoryServiceConnection.UpdateCategory(id, updateCategoryDto);

            return Redirect("/tat-ca-loai.html");
        }
    }
}
