using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _notyfService;

        public ConfigProductController(ILogger<ConfigProductController> logger, IConfigProductServiceConnection configProductServiceConnection, INotyfService notyfService)
        {
            _logger = logger;
            _configProductServiceConnection = configProductServiceConnection;
            _notyfService = notyfService;
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
            updateSizeSpecificProductDto.UserName = User.Identity.Name;

            var a = await _configProductServiceConnection.UpdateSpecificProduct(id, updateSizeSpecificProductDto);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/cap-nhat-san-pham/" + proId);
        }
        [HttpPost]
        [Route("/tao-moi-chi-tiet-size-cho-tung-san-pham")]
        public async Task<IActionResult> Create(Guid proId, CreateSizeSpecificProductDto createSizeSpecificProductDto)
        {
            createSizeSpecificProductDto.UserName = User.Identity.Name;
            var a = await _configProductServiceConnection.CreateSizeSpecificProduct(createSizeSpecificProductDto);
            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }
            return Redirect("/cap-nhat-san-pham/" + proId);
        }

        [HttpPost]
        [Route("/cap-nhat-cau-hinh/")]
        public async Task<IActionResult> EditConfig(ConfigProduct configProduct)
        {
            configProduct.UserName = User.Identity.Name;
            var a = await _configProductServiceConnection.CreateConfigProduct(configProduct);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/cap-nhat-san-pham/" + configProduct.ProId);
        }
    }
}
