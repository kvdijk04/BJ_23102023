using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.ConfigWeb;
using BJ.Contract.ConfigWeb.CreateConfigWeb;
using BJ.Contract.ConfigWeb.UpdateConfigWeb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BJ.Admin.Controllers
{
    public class ConfigWebController : Controller
    {
        private readonly ILogger<ConfigWebController> _logger;
        private readonly IConfigWebServiceConnection _configWebServiceConnection;
        private readonly INotyfService _notyfService;
        public ConfigWebController(ILogger<ConfigWebController> logger, IConfigWebServiceConnection configWebServiceConnection, INotyfService notyfService)
        {
            _logger = logger;
            _configWebServiceConnection = configWebServiceConnection;
            _notyfService = notyfService;
        }
        [Route("/tat-ca-cau-hinh-web.html")]
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
            var r = await _configWebServiceConnection.GetPaging(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("/chi-tiet-cau-hinh-web/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var r = await _configWebServiceConnection.GetConfigWebById(id);
            return View(r);
        }
        [HttpGet]
        [Route("/tao-moi-cau-hinh-web.html")]
        public ActionResult Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-cau-hinh-web.html")]

        public async Task<IActionResult> Create(CreateConfigWebDto createConfigWebDto)
        {
            createConfigWebDto.UserName = User.Identity.Name;

            var a = await _configWebServiceConnection.CreateConfigWeb(createConfigWebDto);
            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }
            return Redirect("/tao-moi-cau-hinh-web.html");
        }
        [HttpGet]
        [Route("/cap-nhat-cau-hinh-web/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var item = await _configWebServiceConnection.GetConfigWebById(id);
            UpdateConfigWebDto updateConfigWebDto = new()
            {
                Name = item.Name,
            };
            ViewBag.ConfigWebId = id;
            return View(updateConfigWebDto);
        }
        [HttpPost]
        [Route("/cap-nhat-cau-hinh-web/{id}")]

        public async Task<IActionResult> Edit(int id,UpdateConfigWebDto updateConfigWebDto)
        {
            updateConfigWebDto.UserName = User.Identity.Name;
            var a = await _configWebServiceConnection.UpdateConfigWeb(id, updateConfigWebDto);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/tat-ca-cau-hinh-web.html");
        }
       
    }
}
