using BJ.Admin.Models;
using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BJ.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductServiceConnection _productService;
        private readonly ISizeServiceConnection _sizeService;
        private readonly ISubCategoryServiceConnection _subCategoryService;
        private readonly ICategoryServiceConnection _categoryService;
        public HomeController(ILogger<HomeController> logger, IProductServiceConnection productService, ISizeServiceConnection sizeService, ISubCategoryServiceConnection subCategoryService, ICategoryServiceConnection categoryService)
        {
            _logger = logger;
            _productService = productService;
            _sizeService = sizeService;
            _subCategoryService = subCategoryService;
            _categoryService = categoryService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}