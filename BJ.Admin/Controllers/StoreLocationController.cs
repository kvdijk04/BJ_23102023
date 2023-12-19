using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.StoreLocation;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Admin.Controllers
{
    public class StoreLocationController : Controller
    {
        private readonly ILogger<StoreLocationController> _logger;
        private readonly IStoreLocationServiceConnection _storeLocationServiceConnection;
        private readonly ILanguageServiceConnection _languageServiceConnection;
        private readonly INotyfService _notyfService;
        public StoreLocationController(ILogger<StoreLocationController> logger, IStoreLocationServiceConnection storeLocationServiceConnection, ILanguageServiceConnection languageServiceConnection, INotyfService notyf)
        {
            _logger = logger;
            _storeLocationServiceConnection = storeLocationServiceConnection;
            _languageServiceConnection = languageServiceConnection;
            _notyfService = notyf;
        }
        [Route("/tat-ca-cua-hang.html")]
        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1)
        {
            if (keyword != null) ViewBag.Keyword = keyword;

            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }
            var request = new GetListPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,

            };
            var r = await _storeLocationServiceConnection.GetPagingStoreLocation(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("chi-tiet-cua-hang/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var r = await _storeLocationServiceConnection.GetStoreById(id);
            return View(r);
        }
        [Route("/tao-moi-cua-hang.html")]
        [HttpGet]
        public IActionResult Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            return View();
        }
        [Route("/tao-moi-cua-hang.html")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateStoreLocationDto createStoreLocationDto)
        {
            var a = await _storeLocationServiceConnection.CreateStoreLocation(createStoreLocationDto);

            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }

            return Redirect("/tao-moi-cua-hang.html");
        }
        [Route("/cap-nhat-cua-hang/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var item = await _storeLocationServiceConnection.GetStoreById(id);
            UpdateStoreLocationDto updateStoreLocationDto = new()
            {
                Address = item.Address,
                City = item.City,
                Closed = item.Closed,
                Repaired = item.Repaired,
                Name = item.Name,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                IconPath = item.IconPath,
                ImagePath = item.ImagePath,

            };
            ViewBag.Id = id;

            return View(updateStoreLocationDto);
        }
        [Route("/cap-nhat-cua-hang/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, float lat, float longLat, [FromForm] UpdateStoreLocationDto updateStoreLocationDto)
        {
            var result = await _storeLocationServiceConnection.GetStoreById(id);

            if (updateStoreLocationDto.ImageStore == null) updateStoreLocationDto.ImagePath = result.ImagePath;

            updateStoreLocationDto.IconPath = result.IconPath;
            updateStoreLocationDto.Latitude = lat;
            updateStoreLocationDto.Longitude = longLat;
            var a = await _storeLocationServiceConnection.UpdateStoreLocation(id, updateStoreLocationDto);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/tat-ca-cua-hang.html");
        }

    }
}
