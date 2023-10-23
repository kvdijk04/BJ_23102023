using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.SubCategory;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Admin.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ILogger<SubCategoryController> _logger;
        private readonly ISubCategoryServiceConnection _subCategoryServiceConnection;
        public SubCategoryController(ILogger<SubCategoryController> logger, ISubCategoryServiceConnection subCategoryServiceConnection)
        {
            _logger = logger;
            _subCategoryServiceConnection = subCategoryServiceConnection;
        }
        [Route("/tat-ca-danh-muc-con.html")]
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
            var r = await _subCategoryServiceConnection.GetPagingSubCategory(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("/chi-tiet-danh-muc-con/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var r = await _subCategoryServiceConnection.GetSubCategoryById(id);

            return View(r);
        }

        [Route("/tao-moi-danh-muc-con.html")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-danh-muc-con.html")]

        public async Task<IActionResult> Create([FromForm] CreateSubCategoryDto createSubCategoryDto)
        {

            var r = await _subCategoryServiceConnection.CreateSubCategory(createSubCategoryDto);

            return Redirect("/tao-moi-danh-muc-con.html");
        }
        [Route("/cap-nhat-danh-muc-con/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _subCategoryServiceConnection.GetSubCategoryById(id);
            UpdateSubCategoryDto updateSubCategoryDto = new()
            {
                Active = item.Active,
                SubCatName = item.SubCatName,
                DateUpdated = item.DateUpdated,
                Description = item.Description,
                ImagePath = item.ImagePath,
            };
            ViewBag.SubCatId = id;

            return View(updateSubCategoryDto);
        }
        [HttpPost]
        [Route("/cap-nhat-danh-muc-con/{id}")]

        public async Task<IActionResult> Edit(int id, [FromForm] UpdateSubCategoryDto updateSubCategoryDto)
        {

            var result = await _subCategoryServiceConnection.GetSubCategoryById(id);

            if (updateSubCategoryDto.Image == null) updateSubCategoryDto.ImagePath = result.ImagePath;

            var r = await _subCategoryServiceConnection.UpdateSubCategory(id, updateSubCategoryDto);

            return Redirect("/tat-ca-danh-muc-con.html");
        }
    }
}
