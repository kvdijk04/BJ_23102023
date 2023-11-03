using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Admin.Controllers
{
    public class ImportController : Controller
    {
        private readonly ILogger<ImportController> _logger;
        private readonly IImportExcelServiceConnection _importExcelService;
        public INotyfService _notyfService { get; }

        public ImportController(ILogger<ImportController> logger, IImportExcelServiceConnection importExcelService, INotyfService notyfService)
        {
            _logger = logger;
            _importExcelService = importExcelService;
            _notyfService = notyfService;
        }
        [Route("/load-file-excel.hmtl", Name = "HomeImport")]
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Import([FromForm] ImportResponse importResponse, IFormFile formFile, CancellationToken cancellationToken, bool category, bool subCategory, bool size, bool product,
                                bool blog, bool news)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            importResponse.File = formFile;
            var rs = await _importExcelService.ImportExcel(importResponse, cancellationToken, category, subCategory, size, product, blog, news);

            if (rs == "Import Success")
            {
                _notyfService.Success("Thêm thành công. Vui lòng kiểm tra lại!");
                return RedirectToRoute("HomeImport");

            }
            else
            {
                _notyfService.Error(rs);
                return RedirectToRoute("HomeImport");
            }


        }
    }
}
