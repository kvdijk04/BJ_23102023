using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.App.Models;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BJ.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoreLocationServiceConnection _storeLocationServiceConnection;
        private readonly INewsServiceConnection _newsServiceConnection;
        private readonly IConfiguration _configuration;
        private readonly IEmailServiceConnection _emailServiceConnection;
        private readonly INotyfService _notyfService;
        public HomeController(ILogger<HomeController> logger, IStoreLocationServiceConnection storeLocationServiceConnection, INewsServiceConnection newsServiceConnection, IConfiguration configuration, IEmailServiceConnection emailServiceConnection, INotyfService notyfService)
        {
            _logger = logger;
            _storeLocationServiceConnection = storeLocationServiceConnection;
            _newsServiceConnection = newsServiceConnection;
            _configuration = configuration;
            _emailServiceConnection = emailServiceConnection;
            _notyfService = notyfService;

        }
        public async Task<IActionResult> Index(string culture)
        {
            if (culture == null) { culture = _configuration.GetValue<string>("DefaultLanguageId"); }
            var news = await _newsServiceConnection.GetNewsAtHome(culture);
            return View(news);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [Route("/getnews")]
        public async Task<IActionResult> GetNews(string culture)
        {
            if (culture == null) { culture = _configuration.GetValue<string>("DefaultLanguageId"); }
            var news = await _newsServiceConnection.GetNewsAtHome(culture);
            return PartialView("_NewsHomePage", news);
        }
        public IActionResult Privacy(string culture)
        {
            if (culture == "vi") return View("Views/Home/Language/vi/CSBM.cshtml");
            return View();
        }
        public IActionResult UsePolicy(string culture)
        {
            if (culture == "vi") return View("Views/Home/Language/vi/CSSD.cshtml");
            return View();
        }
        public IActionResult Delivery()
        {
            return View();
        }
        public IActionResult TermOfUse(string culture)
        {
            if (culture == "vi") return View("Views/Home/Language/vi/DKTT.cshtml");
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<IActionResult> Contact(string reason, string fullname, string email, string phone, string vibe_member, string store_name, string message)
        {
            FeedBack feedBack = new()
            {
                Email = email,
                FullName = fullname,
                Message = message,
                Phone = phone,
                Reason = reason,
                StoreName = store_name,
                VibeMember = vibe_member,
            };
            var a = await _emailServiceConnection.SendEmail(feedBack);

            return RedirectToAction("Contact");
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

    }
}