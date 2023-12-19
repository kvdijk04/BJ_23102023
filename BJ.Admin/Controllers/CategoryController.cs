using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.Category;
using BJ.Contract.Translation.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BJ.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryServiceConnection _categoryServiceConnection;
        private readonly ILanguageServiceConnection _languageServiceConnection;
        private readonly INotyfService _notyfService;
        public CategoryController(ILogger<CategoryController> logger, ICategoryServiceConnection categoryServiceConnection, ILanguageServiceConnection languageServiceConnection, INotyfService notyf)
        {
            _logger = logger;
            _categoryServiceConnection = categoryServiceConnection;
            _languageServiceConnection = languageServiceConnection;
            _notyfService = notyf;
        }
        [Route("/tat-ca-loai.html")]
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
            var r = await _categoryServiceConnection.GetPagingCategory(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("chi-tiet-loai/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var r = await _categoryServiceConnection.GetCategoryById(id);
            return View(r);
        }
        [Route("/tao-moi-loai.html")]
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
        [Route("/tao-moi-loai.html")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCategoryDto createCategoryDto)
        {
            var a = await _categoryServiceConnection.CreateCategory(createCategoryDto);

            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }

            return Redirect("/tao-moi-loai.html");
        }
        [Route("/cap-nhat-loai/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

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
            ViewBag.Id = id;

            return View(updateCategoryDto);
        }
        [Route("/cap-nhat-loai/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [FromForm] UpdateCategoryDto updateCategoryDto)
        {
            var result = await _categoryServiceConnection.GetCategoryById(id);

            if (updateCategoryDto.Image == null) updateCategoryDto.ImagePath = result.ImagePath;

            var a = await _categoryServiceConnection.UpdateCategory(id, updateCategoryDto);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/tat-ca-loai.html");
        }


        [Route("chi-tiet-loai/{catId}/ngon-ngu/{languageId}/xem-chi-tiet")]
        [HttpGet]
        public async Task<IActionResult> LanguageDetail(Guid catId, Guid languageId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var category = await _categoryServiceConnection.GetCategoryById(catId);
            ViewBag.CatName = category.CatName;
            ViewBag.Id = catId;
            ViewBag.LanguageId = languageId;
            var r = await _categoryServiceConnection.GetCategoryTranslationnById(languageId);
            return View(r);
        }
        [Route("chi-tiet-loai/{id}/them-moi-ngon-ngu")]
        [HttpGet]
        public async Task<IActionResult> CreateLanguage(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var category = await _categoryServiceConnection.GetCategoryById(id);
            var language = await _languageServiceConnection.GetAllLanguages();
            ViewData["Language"] = new SelectList(language, "Id", "Name");
            ViewBag.CatName = category.CatName;
            ViewBag.Id = category.Id;
            return View();
        }


        [HttpPost]
        [Route("chi-tiet-loai/{id}/them-moi-ngon-ngu")]

        public async Task<IActionResult> CreateLanguage(Guid id, CreateCategoryTranslationDto createCategoryTranslationDto)
        {
            createCategoryTranslationDto.CategoryId = id;

            var a = await _categoryServiceConnection.CreateLanguage(createCategoryTranslationDto);

            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }

            return Redirect("/chi-tiet-loai/" + id);
        }

        [Route("chi-tiet-loai/{catId}/ngon-ngu/{languageId}/cap-nhat")]
        [HttpGet]
        public async Task<IActionResult> UpdateLanguage(Guid catId, Guid languageId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var r = await _categoryServiceConnection.GetCategoryTranslationnById(languageId);
            var category = await _categoryServiceConnection.GetCategoryById(catId);
            ViewBag.CatName = category.CatName;
            ViewBag.Id = catId;
            ViewBag.LanguageId = languageId;

            UpdateCategoryTranslationDto updateCategoryTranslationDto = new()
            {
                CatName = r.CatName,
                Description = r.Description,
                MetaDesc = r.MetaDesc,

            };
            return View(updateCategoryTranslationDto);
        }


        [HttpPost]
        [Route("chi-tiet-loai/{catId}/ngon-ngu/{languageId}/cap-nhat")]

        public async Task<IActionResult> UpdateLanguage(Guid catId, Guid languageId, UpdateCategoryTranslationDto updateCategoryTranslationDto)
        {
            var a = await _categoryServiceConnection.UpdateCategoryTranslationn(catId, languageId, updateCategoryTranslationDto);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/chi-tiet-loai/" + catId);
        }
    }
}
