using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.ConfigWeb;
using BJ.Contract.ConfigWeb.CreateConfigWeb;
using BJ.Contract.ConfigWeb.UpdateConfigWeb;
using BJ.Contract.Translation.Blog;
using BJ.Contract.Translation.ConfigWeb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BJ.Admin.Controllers
{
    public class DetailConfigWebController : Controller
    {
        private readonly ILogger<DetailConfigWebController> _logger;
        private readonly IDetailConfigWebServiceConnection _detailConfigWebServiceConnection;
        private readonly IConfigWebServiceConnection _configWebServiceConnection;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _notyfService;
        private readonly ILanguageServiceConnection _languageServiceConnection;
        public DetailConfigWebController(ILogger<DetailConfigWebController> logger,ILanguageServiceConnection languageServiceConnection, IConfigWebServiceConnection configWebServiceConnection,IConfiguration configuration, IDetailConfigWebServiceConnection detailConfigWebServiceConnection, INotyfService notyfService)
        {
            _logger = logger;
            _detailConfigWebServiceConnection = detailConfigWebServiceConnection;
            _configWebServiceConnection = configWebServiceConnection;
            _notyfService = notyfService;
            _configuration = configuration;
            _languageServiceConnection = languageServiceConnection;
        }
        [Route("/cau-hinh-trang.html")]
        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int configWebId, int pageIndex = 1)
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
                CategoryId = configWebId

            };
            var r = await _detailConfigWebServiceConnection.GetPaging(request);
            var listCat = await _configWebServiceConnection.GetAllConfigWebs();

            ViewData["ConfigWebId"] = new SelectList(listCat, "Id", "Name");
            ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("/chi-tiet-cau-hinh-trang/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id, string culture)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var r = await _detailConfigWebServiceConnection.GetDetailConfigWebById(id, culture);
            return View(r);
        }
        [HttpGet]
        [Route("/tao-moi-cau-hinh-trang.html")]
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }
            var listCat = await _configWebServiceConnection.GetAllConfigWebs();

            ViewData["ConfigWebId"] = new SelectList(listCat, "Id", "Name");
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-cau-hinh-trang.html")]

        public async Task<IActionResult> Create(CreateConfigWebAdminView createConfigWebAdminView)
        {
            var a = await _detailConfigWebServiceConnection.CreateDetailConfigWeb(createConfigWebAdminView);
            if (a == "OK")
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error(a);
            }
            return Redirect("/tao-moi-cau-hinh-trang.html");
        }
        [HttpGet]
        [Route("/cap-nhat-cau-hinh-trang/{id}")]
        public async Task<IActionResult> Edit(Guid id, string culture)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var item = await _detailConfigWebServiceConnection.GetDetailConfigWebById(id, culture);
            UpdateDetailConfigWebDto updateDetailConfigWebDto = new()
            {
                DateUpdated = DateTime.Now,
                Description = item.Description,
                Title = item.Title,
                Active = item.Active,
                Url = item.Url,
                SortOrder = item.SortOrder,
            };
            ViewBag.DetailConfigWebId = id;
            return View(updateDetailConfigWebDto);
        }
        [HttpPost]
        [Route("/cap-nhat-cau-hinh-trang/{id}")]

        public async Task<IActionResult> Edit(Guid id,UpdateDetailConfigWebDto updateDetailConfigWebDto)
        {
            var a = await _detailConfigWebServiceConnection.UpdateDetailConfigWeb(id, updateDetailConfigWebDto);
            if (a == "OK")
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error(a);
            }
            return Redirect("/cau-hinh-trang.html");
        }

        [Route("chi-tiet-cau-hinh-trang/{detailConfigId}/ngon-ngu/{languageId}/xem-chi-tiet")]
        [HttpGet]
        public async Task<IActionResult> LanguageDetail(Guid detailConfigId, Guid languageId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var culture = _configuration.GetValue<string>("DefaultLanguageId");

            var config = await _detailConfigWebServiceConnection.GetDetailConfigWebById(detailConfigId, culture);
            ViewBag.Id = detailConfigId;
            ViewBag.LanguageId = languageId;

            var r = await _detailConfigWebServiceConnection.GetDetailConfigWebTranslationnById(languageId);

            return View(r);
        }
        [Route("chi-tiet-cau-hinh-trang/{detailConfigId}/them-moi-ngon-ngu")]
        [HttpGet]
        public async Task<IActionResult> CreateLanguage(Guid detailConfigId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var culture = _configuration.GetValue<string>("DefaultLanguageId");
            var config = await _detailConfigWebServiceConnection.GetDetailConfigWebById(detailConfigId, culture);
            var language = await _languageServiceConnection.GetAllLanguages();
            ViewData["Language"] = new SelectList(language, "Id", "Name");
            ViewBag.Title = config.Title;
            ViewBag.Id = config.Id;


            return View();
        }


        [HttpPost]
        [Route("chi-tiet-cau-hinh-trang/{detailConfigId}/them-moi-ngon-ngu")]

        public async Task<IActionResult> CreateLanguage(Guid detailConfigId, CreateDetailConfigWebTranslationDto createDetailConfigWebTranslationDto)
        {
            createDetailConfigWebTranslationDto.DetailConfigWebId = detailConfigId;

            var a = await _detailConfigWebServiceConnection.CreateLanguage(createDetailConfigWebTranslationDto);

            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }

            return Redirect("/chi-tiet-cau-hinh-trang/" + detailConfigId);
        }

        [Route("chi-tiet-cau-hinh-trang/{detailConfigId}/ngon-ngu/{languageId}/cap-nhat")]
        [HttpGet]
        public async Task<IActionResult> UpdateLanguage(Guid detailConfigId, Guid languageId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var culture = _configuration.GetValue<string>("DefaultLanguageId");

            var r = await _detailConfigWebServiceConnection.GetDetailConfigWebTranslationnById(languageId);
            var config = await _detailConfigWebServiceConnection.GetDetailConfigWebById(detailConfigId, culture);

            ViewBag.Title = config.Title;
            ViewBag.Id = config.Id;
            ViewBag.LanguageId = languageId;

            UpdateDetailConfigWebTranslationDto updateDetailConfigWebTranslationDto = new()
            {
                Title = r.Title,
                Description = r.Description,
                Url = r.Url,
            };
            return View(updateDetailConfigWebTranslationDto);
        }


        [HttpPost]
        [Route("chi-tiet-cau-hinh-trang/{detailConfigId}/ngon-ngu/{languageId}/cap-nhat")]

        public async Task<IActionResult> UpdateLanguage(Guid detailConfigId, Guid languageId, UpdateDetailConfigWebTranslationDto updateDetailConfigWebTranslationDto)
        {
            var a = await _detailConfigWebServiceConnection.UpdateDetailConfigWebTranslationn(languageId, updateDetailConfigWebTranslationDto);

            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/chi-tiet-cau-hinh-trang/" + detailConfigId);
        }

    }
}
