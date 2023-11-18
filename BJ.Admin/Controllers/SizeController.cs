using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.Size;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BJ.Admin.Controllers
{
    public class SizeController : Controller
    {
        private readonly ILogger<SizeController> _logger;
        private readonly ISizeServiceConnection _sizeServiceConnection;
        private readonly INotyfService _notyfService;
        private readonly ICategoryServiceConnection _categoryServiceConnection;
        public SizeController(ILogger<SizeController> logger, ISizeServiceConnection sizeServiceConnection, INotyfService notyfService, ICategoryServiceConnection categoryServiceConnection)
        {
            _logger = logger;
            _sizeServiceConnection = sizeServiceConnection;
            _notyfService = notyfService;
            _categoryServiceConnection = categoryServiceConnection;
        }
        [Route("/tat-ca-size.html")]
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
            var r = await _sizeServiceConnection.GetPaging(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("/chi-tiet-size/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }

            var r = await _sizeServiceConnection.GetSizeById(id);
            return View(r);
        }
        [HttpGet]
        [Route("/tao-moi-size.html")]
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var listCat = await _categoryServiceConnection.GetAllCategories();

            ViewData["CategoryId"] = new SelectList(listCat, "Id", "CatName");
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-size.html")]

        public async Task<IActionResult> Create(CreateSizeDto createSizeDto)
        {
            var a = await _sizeServiceConnection.CreateSize(createSizeDto);
            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }
            return Redirect("/tao-moi-size.html");
        }
        [HttpGet]
        [Route("/cap-nhat-size/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }

            var item = await _sizeServiceConnection.GetSizeById(id);
            UpdateSizeDto updateSizeDto = new()
            {
                Active = item.Active,
                Name = item.Name,
                Price = item.Price,
                Note = item.Note
            };
            ViewBag.SizeId = id;
            return View(updateSizeDto);
        }
        [HttpPost]
        [Route("/cap-nhat-size/{id}")]

        public async Task<IActionResult> Edit(int id, UpdateSizeDto updateSizeDto)
        {
            var a = await _sizeServiceConnection.UpdateSize(id, updateSizeDto);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/tat-ca-size.html");
        }
        public async Task<ActionResult> SizeList(Guid catId)
        {
            var result = await _sizeServiceConnection.GetAllSizesByCatId(catId);

            return Json(result);
        }
    }
}
