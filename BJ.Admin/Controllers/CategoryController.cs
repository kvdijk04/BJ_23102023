using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.Category;
using BJ.Contract.Translation.Category;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

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
        public async Task<IActionResult> Create([FromForm] CreateCategoryAdminView createCategoryAdminView)
        {
            createCategoryAdminView.CreateCategoryDto.UserName = User.Identity.Name;

            if(createCategoryAdminView.CreateCategoryDto.DateActiveForm != null && createCategoryAdminView.CreateCategoryDto.DateTimeActiveTo != null)
            {
                int compareBetweenFromAndTo = DateTime.Compare((DateTime)createCategoryAdminView.CreateCategoryDto.DateTimeActiveTo, (DateTime)createCategoryAdminView.CreateCategoryDto.DateActiveForm);

                if (compareBetweenFromAndTo < 0)
                {

                    //TempData["Title"] = createAppointmentSlotAdvance.Title;
                    //TempData["Note"] = createAppointmentSlotAdvance.Note;
                    //TempData["Description"] = createAppointmentSlotAdvance.Description;

                    //TempData["FromDate"] = createAppointmentSlotAdvance.Start;
                    //TempData["ToDate"] = createAppointmentSlotAdvance.End;
                    //TempData["CustomAdd"] = createAppointmentSlotAdvance.TypedSubmit;


                    _notyfService.Error("Thời gian không hợp lệ");
                    return Redirect("/tao-moi-loai.html");
                }
            }

            var a = await _categoryServiceConnection.CreateCategory(createCategoryAdminView);

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
            UpdateCategoryAdminView updateCategoryAdminView= new()
            {
                UpdateCategory = new()
                {
                    Active = item.Active,
                    
                    DateUpdated = item.DateUpdated,
                    ImagePath = item.ImagePath,
                    Sort = item.Sort,
                    DateActiveForm = item.DateActiveForm,
                    DateTimeActiveTo = item.DateTimeActiveTo,
                },
                UpdateCategoryTranslationDto = new()
                {
                    Alias = item.Alias,
                    CatName = item.CatName,
                    Description = item.Description,

                },

            };
            ViewBag.Id = id;

            return View(updateCategoryAdminView);
        }
        [Route("/cap-nhat-loai/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [FromForm] UpdateCategoryAdminView updateCategoryAdminView)
        {
            updateCategoryAdminView.UpdateCategory.UserName = User.Identity.Name;
            if (updateCategoryAdminView.UpdateCategory.DateActiveForm != null && updateCategoryAdminView.UpdateCategory.DateTimeActiveTo != null)
            {
                int compareBetweenFromAndTo = DateTime.Compare((DateTime)updateCategoryAdminView.UpdateCategory.DateTimeActiveTo, (DateTime)updateCategoryAdminView.UpdateCategory.DateActiveForm);

                if (compareBetweenFromAndTo < 0)
                {

                    //TempData["Title"] = createAppointmentSlotAdvance.Title;
                    //TempData["Note"] = createAppointmentSlotAdvance.Note;
                    //TempData["Description"] = createAppointmentSlotAdvance.Description;

                    //TempData["FromDate"] = createAppointmentSlotAdvance.Start;
                    //TempData["ToDate"] = createAppointmentSlotAdvance.End;
                    //TempData["CustomAdd"] = createAppointmentSlotAdvance.TypedSubmit;


                    _notyfService.Error("Thời gian không hợp lệ");
                    return Redirect("/cap-nhat-loai/" + id);
                }
            }
                
            var result = await _categoryServiceConnection.GetCategoryById(id);

            if (updateCategoryAdminView.Image == null) updateCategoryAdminView.UpdateCategory.ImagePath = result.ImagePath;

            var a = await _categoryServiceConnection.UpdateCategory(id, updateCategoryAdminView);
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
            var r = await _categoryServiceConnection.GetCategoryTranslationById(languageId);
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
            createCategoryTranslationDto.UserName = User.Identity.Name;
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

            var r = await _categoryServiceConnection.GetCategoryTranslationById(languageId);
            var category = await _categoryServiceConnection.GetCategoryById(catId);
            ViewBag.CatName = category.CatName;
            ViewBag.Id = catId;
            ViewBag.LanguageId = languageId;

            UpdateCategoryTranslationDto updateCategoryTranslationDto = new()
            {
                CatName = r.CatName,
                Description = r.Description,

            };
            return View(updateCategoryTranslationDto);
        }


        [HttpPost]
        [Route("chi-tiet-loai/{catId}/ngon-ngu/{languageId}/cap-nhat")]

        public async Task<IActionResult> UpdateLanguage(Guid catId, Guid languageId, UpdateCategoryTranslationDto updateCategoryTranslationDto)
        {
            updateCategoryTranslationDto.UserName = User.Identity.Name;

            var a = await _categoryServiceConnection.UpdateCategoryTranslation(languageId, updateCategoryTranslationDto);
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
