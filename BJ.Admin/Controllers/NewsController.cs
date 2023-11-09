using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.News;
using BJ.Contract.Translation.News;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BJ.Admin.Controllers
{
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewsServiceConnection _blogServiceConnection;
        private readonly IConfiguration _configuration;
        private readonly ILanguageServiceConnection _languageService;
        private readonly INotyfService _notyfService;
        public NewsController(ILogger<NewsController> logger, INewsServiceConnection blogServiceConnection, IConfiguration configuration, ILanguageServiceConnection languageService, INotyfService notyfService)
        {
            _logger = logger;
            _blogServiceConnection = blogServiceConnection;
            _configuration = configuration;
            _languageService = languageService;
            _notyfService = notyfService;

        }
        [Route("/tat-ca-tin-tuc.html")]
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
            var r = await _blogServiceConnection.GetPaging(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("/chi-tiet-tin-tuc/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            var r = await _blogServiceConnection.GetNewsById(id, defaultLanguage);
            return View(r);
        }
        [HttpGet]
        [Route("/tao-moi-tin-tuc.html")]
        public IActionResult Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-tin-tuc.html")]

        public async Task<IActionResult> Create([FromForm] CreateNewsAdminView createNewsAdminView)
        {
            var a = await _blogServiceConnection.CreateNews(createNewsAdminView);

            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }


            return Redirect("/tao-moi-tin-tuc.html");
        }
        [HttpGet]
        [Route("/cap-nhat-tin-tuc/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            var item = await _blogServiceConnection.GetNewsById(id, defaultLanguage);
            UpdateNewsAdminView updateNewsAdminView = new()
            {
                UpdateNews = new UpdateNewsDto()
                {
                    Active = item.Active,
                    DateUpdated = item.DateUpdated,
                    ImagePath = item.ImagePath,
                    Popular = item.Popular,
                    Home = item.Home,
                    Promotion = item.Promotion,
                },
                UpdateNewsTranslation = new Contract.Translation.News.UpdateNewsTranslationDto()
                {
                    Title = item.Title,
                    Alias = item.Alias,
                    Description = item.Description,
                    MetaDesc = item.MetaDesc,
                    MetaKey = item.MetaKey,
                    ShortDesc = item.ShortDesc,
                },

            };
            ViewBag.NewsId = id;
            return View(updateNewsAdminView);
        }
        [HttpPost]
        [Route("/cap-nhat-tin-tuc/{id}")]
        public async Task<IActionResult> Edit(Guid id, UpdateNewsAdminView updateNewsAdminView)
        {
            var culture = _configuration.GetValue<string>("DefaultLanguageId");
            var item = await _blogServiceConnection.GetNewsById(id, culture);
            if (updateNewsAdminView.FileUpload == null) { updateNewsAdminView.UpdateNews.ImagePath = item.ImagePath; }

            var a = await _blogServiceConnection.UpdateNews(id, culture, updateNewsAdminView);

            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }

            return Redirect("/tat-ca-tin-tuc.html");
        }

        [Route("chi-tiet-tin-tuc/{blogId}/ngon-ngu/{languageId}/xem-chi-tiet")]
        [HttpGet]
        public async Task<IActionResult> LanguageDetail(Guid blogId, Guid languageId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var culture = _configuration.GetValue<string>("DefaultLanguageId");

            var news = await _blogServiceConnection.GetNewsById(blogId, culture);
            ViewBag.Id = blogId;
            ViewBag.LanguageId = languageId;

            var r = await _blogServiceConnection.GetNewsTranslationnById(languageId);
            return View(r);
        }
        [Route("chi-tiet-tin-tuc/{id}/them-moi-ngon-ngu")]
        [HttpGet]
        public async Task<IActionResult> CreateLanguage(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var culture = _configuration.GetValue<string>("DefaultLanguageId");
            var news = await _blogServiceConnection.GetNewsById(id, culture);
            var language = await _languageService.GetAllLanguages();
            ViewData["Language"] = new SelectList(language, "Id", "Name");
            ViewBag.Title = news.Title;
            ViewBag.Id = news.Id;
            return View();
        }


        [HttpPost]
        [Route("chi-tiet-tin-tuc/{id}/them-moi-ngon-ngu")]

        public async Task<IActionResult> CreateLanguage(Guid id, CreateNewsTranslationDto createNewsTranslationDto)
        {
            createNewsTranslationDto.NewsId = id;

            var a = await _blogServiceConnection.CreateLanguage(createNewsTranslationDto);

            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }

            return Redirect("/chi-tiet-tin-tuc/" + id);
        }

        [Route("chi-tiet-tin-tuc/{blogId}/ngon-ngu/{languageId}/cap-nhat")]
        [HttpGet]
        public async Task<IActionResult> UpdateLanguage(Guid blogId, Guid languageId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var culture = _configuration.GetValue<string>("DefaultLanguageId");

            var r = await _blogServiceConnection.GetNewsTranslationnById(languageId);
            var news = await _blogServiceConnection.GetNewsById(blogId, culture);
            ViewBag.Title = news.Title;
            ViewBag.Id = blogId;
            ViewBag.LanguageId = languageId;

            UpdateNewsTranslationDto updateNewsTranslationDto = new()
            {
                Title = r.Title,
                Description = r.Description,
                MetaDesc = r.MetaDesc,
                MetaKey = r.MetaKey,
                ShortDesc = r.ShortDesc,
            };
            return View(updateNewsTranslationDto);
        }


        [HttpPost]
        [Route("chi-tiet-tin-tuc/{blogId}/ngon-ngu/{languageId}/cap-nhat")]

        public async Task<IActionResult> UpdateLanguage(Guid blogId, Guid languageId, UpdateNewsTranslationDto updateNewsTranslationDto)
        {
            var a = await _blogServiceConnection.UpdateNewsTranslationn(languageId, updateNewsTranslationDto);

            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }

            return Redirect("/chi-tiet-tin-tuc/" + blogId);
        }
    }
}
