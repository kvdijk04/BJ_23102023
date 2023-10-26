using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.SubCategory;
using BJ.Contract.Translation.SubCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BJ.Admin.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ILogger<SubCategoryController> _logger;
        private readonly ISubCategoryServiceConnection _subCategoryServiceConnection;
        private readonly ILanguageServiceConnection _languageServiceConnection;
        public SubCategoryController(ILogger<SubCategoryController> logger, ISubCategoryServiceConnection subCategoryServiceConnection, ILanguageServiceConnection languageServiceConnection)
        {
            _logger = logger;
            _subCategoryServiceConnection = subCategoryServiceConnection;
            _languageServiceConnection = languageServiceConnection;

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
            ViewBag.Id = r.Id;
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
            ViewBag.Id = id;

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


        [Route("chi-tiet-danh-muc-con/{subCatId}/ngon-ngu/{languageId}/xem-chi-tiet")]
        [HttpGet]
        public async Task<IActionResult> LanguageDetail(int subCatId, Guid languageId)
        {
            var category = await _subCategoryServiceConnection.GetSubCategoryById(subCatId);
            ViewBag.SubCatName = category.SubCatName;
            ViewBag.Id = subCatId;
            ViewBag.LanguageId = languageId;
            var r = await _subCategoryServiceConnection.GetSubCategoryTranslationnById(languageId);
            return View(r);
        }
        [Route("chi-tiet-danh-muc-con/{id}/them-moi-ngon-ngu")]
        [HttpGet]
        public async Task<IActionResult> CreateLanguage(int id)
        {
            var category = await _subCategoryServiceConnection.GetSubCategoryById(id);
            var language = await _languageServiceConnection.GetAllLanguages();
            ViewData["Language"] = new SelectList(language, "Id", "Name");
            ViewBag.SubCatName = category.SubCatName;
            ViewBag.Id = category.Id;
            return View();
        }


        [HttpPost]
        [Route("chi-tiet-danh-muc-con/{id}/them-moi-ngon-ngu")]

        public async Task<IActionResult> CreateLanguage(int id, CreateSubCategoryTranslationDto createSubCategoryTranslationDto)
        {
            createSubCategoryTranslationDto.SubCategoryId = id;

            await _subCategoryServiceConnection.CreateLanguage(createSubCategoryTranslationDto);

            return Redirect("/chi-tiet-danh-muc-con/" + id);
        }

        [Route("chi-tiet-danh-muc-con/{subCatId}/ngon-ngu/{languageId}/cap-nhat")]
        [HttpGet]
        public async Task<IActionResult> UpdateLanguage(int subCatId, Guid languageId)
        {
            var r = await _subCategoryServiceConnection.GetSubCategoryTranslationnById(languageId);
            var category = await _subCategoryServiceConnection.GetSubCategoryById(subCatId);
            ViewBag.SubCatName = category.SubCatName;
            ViewBag.Id = subCatId;
            ViewBag.LanguageId = languageId;

            UpdateSubCategoryTranslationDto updateCategoryTranslationDto = new()
            {
                SubCatName = r.SubCatName,
                Description = r.Description,

            };
            return View(updateCategoryTranslationDto);
        }


        [HttpPost]
        [Route("chi-tiet-danh-muc-con/{subCatId}/ngon-ngu/{languageId}/cap-nhat")]

        public async Task<IActionResult> UpdateLanguage(int subCatId, Guid languageId, UpdateSubCategoryTranslationDto updateSubCategoryTranslationDto)
        {
            await _subCategoryServiceConnection.UpdateSubCategoryTranslationn(subCatId, languageId, updateSubCategoryTranslationDto);

            return Redirect("/chi-tiet-danh-muc-con/" + subCatId);
        }
    }
}
