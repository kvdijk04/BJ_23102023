using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.Size;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Admin.Controllers
{
    public class SizeController : Controller
    {
        private readonly ILogger<SizeController> _logger;
        private readonly ISizeServiceConnection _sizeServiceConnection;
        public SizeController(ILogger<SizeController> logger, ISizeServiceConnection sizeServiceConnection)
        {
            _logger = logger;
            _sizeServiceConnection = sizeServiceConnection;
        }
        [Route("/tat-ca-size.html")]
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
            var r = await _sizeServiceConnection.GetPaging(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("/chi-tiet-size/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var r = await _sizeServiceConnection.GetSizeById(id);
            return View(r);
        }
        [HttpGet]
        [Route("/tao-moi-size.html")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-size.html")]

        public async Task<IActionResult> Create(CreateSizeDto createSizeDto)
        {
            var a = await _sizeServiceConnection.CreateSize(createSizeDto);

            return Redirect("/tao-moi-size.html");
        }
        [HttpGet]
        [Route("/cap-nhat-size/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _sizeServiceConnection.GetSizeById(id);
            UpdateSizeDto updateSizeDto = new()
            {
                Active = item.Active,
                Name = item.Name,
                Price = item.Price,

            };
            ViewBag.SizeId = id;
            return View(updateSizeDto);
        }
        [HttpPost]
        [Route("/cap-nhat-size/{id}")]

        public async Task<IActionResult> Edit(int id, UpdateSizeDto updateSizeDto)
        {
            var a = await _sizeServiceConnection.UpdateSize(id, updateSizeDto);

            return Redirect("/tat-ca-size.html");
        }
    }
}
