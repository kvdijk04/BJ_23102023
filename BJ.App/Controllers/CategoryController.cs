using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryServiceConnection _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryServiceConnection categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }


    }
}
