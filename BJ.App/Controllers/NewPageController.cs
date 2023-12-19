using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class NewPageController : Controller
    {
        private readonly ILogger<NewPageController> _logger;
        private readonly IDetailConfigWebServiceConnection _configWebServiceConnection;
        private readonly INotyfService _notyfService;
        public NewPageController(ILogger<NewPageController> logger, IDetailConfigWebServiceConnection configWebServiceConnection, INotyfService notyfService)
        {
            _logger = logger;
            _configWebServiceConnection = configWebServiceConnection;
            _notyfService = notyfService;
        }


        public async Task<IActionResult> Index(string culture, string url)
        {
            var page = await _configWebServiceConnection.GetDetailConfigWebByUrl(url, culture);
            return View(page);
        }
    }
}
