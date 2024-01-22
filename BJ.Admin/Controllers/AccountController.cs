using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;

namespace BJ.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountServiceConnection _accountServiceConnection;
        private readonly INotyfService _notyfService;
        private readonly ICategoryServiceConnection _categoryServiceConnection;
        public AccountController(ILogger<AccountController> logger, IAccountServiceConnection accountServiceConnection, INotyfService notyfService, ICategoryServiceConnection categoryServiceConnection)
        {
            _logger = logger;
            _accountServiceConnection = accountServiceConnection;
            _notyfService = notyfService;
            _categoryServiceConnection = categoryServiceConnection;
        }
        [Route("/tat-ca-tai-khoan.html")]
        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1)
        {
            if (keyword != null) ViewBag.Keyword = keyword;

            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }
            var request = new GetListPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,

            };
            var r = await _accountServiceConnection.GetPaging(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("/chi-tiet-tai-khoan/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var r = await _accountServiceConnection.GetAccountById(id);
            return View(r);
        }
        [HttpGet]
        [Route("/tao-moi-tai-khoan.html")]
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }
            var listCat = await _categoryServiceConnection.GetAllCategories();

            ViewData["CategoryId"] = new SelectList(listCat, "Id", "CatName");
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-tai-khoan.html")]

        public async Task<IActionResult> Create(CreateAccountDto createAccountDto)
        {
            createAccountDto.UserName = User.Identity.Name;
            var a = await _accountServiceConnection.CreateAccount(createAccountDto);
            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }
            return Redirect("/tao-moi-tai-khoan.html");
        }
        [HttpGet]
        [Route("/cap-nhat-tai-khoan/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var item = await _accountServiceConnection.GetAccountById(id);
            UpdateAccountDto updateAccountDto = new()
            {
                Active = item.Active,
                EmployeeName = item.EmployeeName,
                UserName = item.UserName,
                HasedPassword = item.HasedPassword,
            };
            ViewBag.AccountId = id;
            return View(updateAccountDto);
        }
        [HttpPost]
        [Route("/cap-nhat-tai-khoan/{id}")]

        public async Task<IActionResult> Edit(Guid id, UpdateAccountDto updateAccountDto)
        {
            updateAccountDto.UserName = User.Identity.Name;

            var a = await _accountServiceConnection.UpdateAccount(id, updateAccountDto);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            
            return Redirect("/tat-ca-tai-khoan.html");
        }
        public async Task<ActionResult> AccountList(Guid catId)
        {
            var result = await _accountServiceConnection.GetAllAccountsByCatId(catId);

            return Json(result);
        }
        [HttpGet]
        [Route("/thong-tin-tai-khoan")]
        public async Task<IActionResult> UserInfo()
        {
            var token = HttpContext.Session.GetString("Token");

            if (User.Identity.Name == null || token == null || User.Claims.Where(x => x.Type == "FullName").Select(x => x.Value).FirstOrDefault() == null) return Redirect("/dang-nhap.html");
            var info = await _accountServiceConnection.GetAccountByEmail(User.Identity.Name);
            ChangePassword changePassword= new()
            {
                EmployeeName = info.EmployeeName,
                UserName = info.UserName,                
            };
            return View(changePassword);
        }
        [HttpPost]
        [Route("/thong-tin-tai-khoan")]
        public async Task<IActionResult> UserInfo(string email, ChangePassword changePassword)
        {
            if (User.Identity.Name == null) return Redirect("/dang-nhap.html");

            email = User.Identity.Name;

            var a = await _accountServiceConnection.ChangePassword(email, changePassword);

            if (a == true)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.Session.Remove("Token");

                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }

            return Redirect("/dang-nhap.html");
        }
    }
}
