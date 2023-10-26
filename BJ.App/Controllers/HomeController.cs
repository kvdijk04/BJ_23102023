using BJ.ApiConnection.Services;
using BJ.App.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BJ.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoreLocationServiceConnection _storeLocationServiceConnection;

        public HomeController(ILogger<HomeController> logger, IStoreLocationServiceConnection storeLocationServiceConnection)
        {
            _logger = logger;
            _storeLocationServiceConnection = storeLocationServiceConnection;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("/{culture}/about.html")]
        public IActionResult About()
        {
            return View();
        }
        [Route("/{culture}/contact.html")]
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Healthy()
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
        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
        [Route("/store")]
        public async Task<JsonResult> GetStore()
        {
            var store = await _storeLocationServiceConnection.GetAllStoreLocations();

            return Json(store);
        }
    }
}