using BJ.ApiConnection.Services;
using BJ.Contract.Size;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Admin.Controllers
{
    public class ConfigProductController : Controller
    {
        private readonly ILogger<ConfigProductController> _logger;
        private readonly IConfigProductServiceConnection _configProductServiceConnection;
        public ConfigProductController(ILogger<ConfigProductController> logger, IConfigProductServiceConnection configProductServiceConnection)
        {
            _logger = logger;
            _configProductServiceConnection = configProductServiceConnection;
        }
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        [Route("/cap-nhat-chi-tiet-size-cho-tung-san-pham")]
        public async Task<IActionResult> Edit(Guid proId, Guid id, UpdateSizeSpecificProductDto updateSizeSpecificProductDto)
        {
            var a = await _configProductServiceConnection.UpdateSpecificProduct(id, updateSizeSpecificProductDto);
            return Redirect("/cap-nhat-san-pham/" + proId);
        }
        [HttpPost]
        [Route("/tao-moi-chi-tiet-size-cho-tung-san-pham")]
        public async Task<IActionResult> Create(Guid proId, CreateSizeSpecificProductDto createSizeSpecificProductDto)
        {
            var a = await _configProductServiceConnection.CreateSizeSpecificProduct(createSizeSpecificProductDto);

            return Redirect("/cap-nhat-san-pham/" + proId);
        }

        [HttpPost]
        [Route("/cap-nhat-cau-hinh/")]
        public async Task<IActionResult> EditConfig(ConfigProduct configProduct)
        {
            var a = await _configProductServiceConnection.CreateConfigProduct(configProduct);

            return Redirect("/cap-nhat-san-pham/" + configProduct.ProId);
        }
    }
}
