using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Contract.Account;
using BJ.Contract.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BJ.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly AppSetting _appSettings;
        private readonly ILoginServiceConnection _loginService;
        private readonly INotyfService _notyf;

        public LoginController(ILogger<LoginController> logger, ILoginServiceConnection loginService, IOptionsMonitor<AppSetting> optionsMonitor, INotyfService notyf)
        {
            _logger = logger;
            _appSettings = optionsMonitor.CurrentValue;
            _loginService = loginService;
            _notyf = notyf;

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("/dang-nhap.html", Name = "Login")]
        public IActionResult Index(LoginDto loginDto)
        {
            return View(loginDto);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAccount(LoginDto loginDto, string returnUrl = null)
        {

            if (!ModelState.IsValid) return View(ModelState);


            var token = await _loginService.Login(loginDto);
            if (token == "" || token == null)
            {
                TempData["Error"] = "Tài khoản không đúng";
                return Redirect("/dang-nhap.html");
            }
            var claim = new List<Claim>
            {
            new Claim(ClaimTypes.Email,loginDto.UserName),
            new Claim(ClaimTypes.GivenName,loginDto.UserName),
            new Claim(ClaimTypes.Name,loginDto.UserName)
            };
            var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            var principle = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, props).Wait();
            HttpContext.Session.SetString("Token", token);

            _notyf.Success("Đăng nhập thảnh công");
            return RedirectToLocal(returnUrl);



        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");

        }
        [HttpPost]
        [Route("/dang-xuat.html")]
        [AllowAnonymous]
        public async Task<string> LogOutAccount()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");

            string result = "done";

            return result;
        }
    }
}
