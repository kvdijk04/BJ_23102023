using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.Blog;
using BJ.Contract.Translation.Blog;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BJ.Admin.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IBlogServiceConnection _blogServiceConnection;
        private readonly IConfiguration _configuration;
        private readonly ILanguageServiceConnection _languageService;
        private readonly INotyfService _notyfService;
        public BlogController(ILogger<BlogController> logger, IBlogServiceConnection blogServiceConnection, IConfiguration configuration, ILanguageServiceConnection languageService, INotyfService notyfService)
        {
            _logger = logger;
            _blogServiceConnection = blogServiceConnection;
            _configuration = configuration;
            _languageService = languageService;
            _notyfService = notyfService;

        }
        [Route("/tat-ca-blog.html")]
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

            ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("/chi-tiet-blog/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            var r = await _blogServiceConnection.GetBlogById(id, defaultLanguage);
            return View(r);
        }
        [HttpGet]
        [Route("/tao-moi-blog.html")]
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
        [Route("/tao-moi-blog.html")]

        public async Task<IActionResult> Create([FromForm] CreateBlogAdminView createBlogAdminView)
        {
            createBlogAdminView.CreateBlog.UserName = User.Identity.Name;

            if (createBlogAdminView.CreateBlog.DateActiveForm != null && createBlogAdminView.CreateBlog.DateTimeActiveTo != null)
            {
                int compareBetweenFromAndTo = DateTime.Compare((DateTime)createBlogAdminView.CreateBlog.DateTimeActiveTo, (DateTime)createBlogAdminView.CreateBlog.DateActiveForm);

                if (compareBetweenFromAndTo < 0)
                {
                    _notyfService.Error("Thời gian không hợp lệ");
                    return Redirect("/tao-moi-blog.html");
                }
            }

            var a = await _blogServiceConnection.CreateBlog(createBlogAdminView);

            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }
            return Redirect("/tao-moi-blog.html");
        }
        [HttpGet]
        [Route("/cap-nhat-blog/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            var item = await _blogServiceConnection.GetBlogById(id, defaultLanguage);
            UpdateBlogAdminView updateBlogAdminView = new()
            {
                UpdateBlog = new UpdateBlogDto()
                {
                    Active = item.Active,
                    DateUpdated = item.DateUpdated,
                    ImagePath = item.ImagePath,
                    Popular = item.Popular,
                    DateActiveForm = item.DateActiveForm,
                    DateTimeActiveTo = item.DateTimeActiveTo,
                },
                UpdateBlogTranslation = new Contract.Translation.Blog.UpdateBlogTranslationDto()
                {
                    Title = item.Title,
                    Alias = item.Alias,
                    Description = item.Description,
                    MetaDesc = item.MetaDesc,
                    MetaKey = item.MetaKey,
                    ShortDesc = item.ShortDesc,
                },

            };
            ViewBag.BlogId = id;
            return View(updateBlogAdminView);
        }
        [HttpPost]
        [Route("/cap-nhat-blog/{id}")]

        public async Task<IActionResult> Edit(Guid id, UpdateBlogAdminView updateBlogAdminView)
        {
            var culture = _configuration.GetValue<string>("DefaultLanguageId");
            var item = await _blogServiceConnection.GetBlogById(id, culture);
            if (updateBlogAdminView.FileUpload == null) { updateBlogAdminView.UpdateBlog.ImagePath = item.ImagePath; }
            updateBlogAdminView.UpdateBlog.UserName = User.Identity.Name;
            if (updateBlogAdminView.UpdateBlog.DateActiveForm != null && updateBlogAdminView.UpdateBlog.DateTimeActiveTo != null)
            {
                int compareBetweenFromAndTo = DateTime.Compare((DateTime)updateBlogAdminView.UpdateBlog.DateTimeActiveTo, (DateTime)updateBlogAdminView.UpdateBlog.DateActiveForm);

                if (compareBetweenFromAndTo < 0)
                {
                    _notyfService.Error("Thời gian không hợp lệ");
                    return Redirect("/cap-nhat-blog/" + id);
                }
            }
            var a = await _blogServiceConnection.UpdateBlog(id, culture, updateBlogAdminView);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/cap-nhat-blog/" + id);
        }

        [Route("chi-tiet-blog/{blogId}/ngon-ngu/{languageId}/xem-chi-tiet")]
        [HttpGet]
        public async Task<IActionResult> LanguageDetail(Guid blogId, Guid languageId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var culture = _configuration.GetValue<string>("DefaultLanguageId");

            var blog = await _blogServiceConnection.GetBlogById(blogId, culture);
            ViewBag.Id = blogId;
            ViewBag.LanguageId = languageId;

            var r = await _blogServiceConnection.GetBlogTranslationById(languageId);
            return View(r);
        }
        [Route("chi-tiet-blog/{id}/them-moi-ngon-ngu")]
        [HttpGet]
        public async Task<IActionResult> CreateLanguage(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var culture = _configuration.GetValue<string>("DefaultLanguageId");
            var blog = await _blogServiceConnection.GetBlogById(id, culture);
            var language = await _languageService.GetAllLanguages();
            ViewData["Language"] = new SelectList(language, "Id", "Name");
            ViewBag.Title = blog.Title;
            ViewBag.Id = blog.Id;


            return View();
        }


        [HttpPost]
        [Route("chi-tiet-blog/{id}/them-moi-ngon-ngu")]

        public async Task<IActionResult> CreateLanguage(Guid id, CreateBlogTranslationDto createBlogTranslationDto)
        {
            createBlogTranslationDto.BlogId = id;
            createBlogTranslationDto.UserName = User.Identity.Name;

            var a = await _blogServiceConnection.CreateLanguage(createBlogTranslationDto);

            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }

            return Redirect("/chi-tiet-blog/" + id);
        }

        [Route("chi-tiet-blog/{blogId}/ngon-ngu/{languageId}/cap-nhat")]
        [HttpGet]
        public async Task<IActionResult> UpdateLanguage(Guid blogId, Guid languageId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var culture = _configuration.GetValue<string>("DefaultLanguageId");

            var r = await _blogServiceConnection.GetBlogTranslationById(languageId);
            var blog = await _blogServiceConnection.GetBlogById(blogId, culture);
            ViewBag.Title = blog.Title;
            ViewBag.Id = blogId;
            ViewBag.LanguageId = languageId;

            UpdateBlogTranslationDto updateBlogTranslationDto = new()
            {
                Title = r.Title,
                Description = r.Description,
                MetaDesc = r.MetaDesc,
                MetaKey = r.MetaKey,
                ShortDesc = r.ShortDesc,
            };
            return View(updateBlogTranslationDto);
        }


        [HttpPost]
        [Route("chi-tiet-blog/{blogId}/ngon-ngu/{languageId}/cap-nhat")]

        public async Task<IActionResult> UpdateLanguage(Guid blogId, Guid languageId, UpdateBlogTranslationDto updateBlogTranslationDto)
        {
            updateBlogTranslationDto.UserName = User.Identity.Name;

            var a = await _blogServiceConnection.UpdateBlogTranslation(languageId, updateBlogTranslationDto);

            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/chi-tiet-blog/" + blogId);
        }
    }
}
