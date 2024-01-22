using AspNetCoreHero.ToastNotification.Abstractions;
using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.StoreLocation;
using BJ.Contract.Translation.Product;
using BJ.Contract.Translation.Store;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;

namespace BJ.Admin.Controllers
{
    public class StoreLocationController : Controller
    {
        private readonly ILogger<StoreLocationController> _logger;
        private readonly IStoreLocationServiceConnection _storeLocationServiceConnection;
        private readonly INotyfService _notyfService;
        private readonly ILanguageServiceConnection _languageServiceConnection;
        public StoreLocationController(ILogger<StoreLocationController> logger, IStoreLocationServiceConnection storeLocationServiceConnection, INotyfService notyf, ILanguageServiceConnection languageServiceConnection)
        {
            _logger = logger;
            _storeLocationServiceConnection = storeLocationServiceConnection;
            _notyfService = notyf;
            _languageServiceConnection = languageServiceConnection;
        }
        [Route("/tat-ca-cua-hang.html")]
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
            var r = await _storeLocationServiceConnection.GetPagingStoreLocation(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        [Route("chi-tiet-cua-hang/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var r = await _storeLocationServiceConnection.GetStoreById(id);
            return View(r);
        }
        [Route("/tao-moi-cua-hang.html")]
        [HttpGet]
        public IActionResult Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            return View();
        }
        [Route("/tao-moi-cua-hang.html")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateStoreLocationAdminView createStoreLocationAdminView)
        {
            createStoreLocationAdminView.CreateStoreLocationDto.UserName = User.Identity.Name;

            var a = await _storeLocationServiceConnection.CreateStoreLocation(createStoreLocationAdminView);

            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }

            return Redirect("/tao-moi-cua-hang.html");
        }
        [Route("/cap-nhat-cua-hang/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var item = await _storeLocationServiceConnection.GetStoreById(id);
            UpdateStoreLocationAdminView updateStoreLocationAdminView= new()
            {
                UpdateStoreLocationDto = new()
                {

                    Closed = item.Closed,
                    Repaired = item.Repaired,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    IconPath = item.IconPath,
                    ImagePath = item.ImagePath,
                    Sort = item.Sort,
                    OpeningSoon = item.OpeningSoon,
                   
                },
                UpdateStoreLocationTranslationDto = new()
                {
                    Address = item.Address,
                    City = item.City,
                    Name = item.Name,
                }

                
            };
            ViewBag.Id = id;

            return View(updateStoreLocationAdminView);
        }
        [Route("/cap-nhat-cua-hang/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, float lat, float longLat, [FromForm] UpdateStoreLocationAdminView updateStoreLocationAdminView)
        {
            var result = await _storeLocationServiceConnection.GetStoreById(id);

            if (updateStoreLocationAdminView.ImageStore == null) updateStoreLocationAdminView.UpdateStoreLocationDto.ImagePath = result.ImagePath;

            updateStoreLocationAdminView.UpdateStoreLocationDto.IconPath = result.IconPath;
            updateStoreLocationAdminView.UpdateStoreLocationDto.Latitude = lat;
            updateStoreLocationAdminView.UpdateStoreLocationDto.Longitude = longLat;
            var a = await _storeLocationServiceConnection.UpdateStoreLocation(id, updateStoreLocationAdminView);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/tat-ca-cua-hang.html");
        }
        [Route("chi-tiet-cua-hang/{id}/them-moi-thoi-gian")]
        [HttpGet]
        public async Task<IActionResult> CreateTimeLine(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }
            var r = await _storeLocationServiceConnection.GetStoreById(id);

            ViewBag.Id = r.Id;

            ViewBag.Title = r.Name;

            return View();
        }
        [Route("chi-tiet-cua-hang/{id}/them-moi-thoi-gian")]
        [HttpPost]
        public async Task<IActionResult> CreateTimeLine(int id, CreateStoreLocationOpenHourDto createStoreLocationTimeLineDto)
        {
            createStoreLocationTimeLineDto.StoreLocationId = id;
            createStoreLocationTimeLineDto.UserName = User.Identity.Name;

            var a = await _storeLocationServiceConnection.CreateStoreLocationTimeLine(createStoreLocationTimeLineDto);

            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }


            return Redirect("/chi-tiet-cua-hang/" + id);
        }

        [Route("chi-tiet-cua-hang/{id}/thoi-gian/{timeId}/cap-nhat")]
        [HttpGet]
        public async Task<IActionResult> UpdateTimeLine(int id, int timeId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }
            var r = await _storeLocationServiceConnection.GetStoreTimeLineById(timeId);
            var product = await _storeLocationServiceConnection.GetStoreById(id);
            ViewBag.Id = id;
            ViewBag.Title = r.DaysOfWeek;

            UpdateStoreLocationOpenHourDto updateStoreLocationTimeLineDto = new()
            {
                Active = r.Active,
                DateUpdated = r.DateUpdated,
                DaysOfWeek = r.DaysOfWeek,
                End = r.End,
                Start = r.Start,
            };
            return View(updateStoreLocationTimeLineDto);
        }

        [Route("chi-tiet-cua-hang/{id}/thoi-gian/{timeId}/cap-nhat")]
        [HttpPost]

        public async Task<IActionResult> UpdateTimeLine(int id, int timeId, UpdateStoreLocationOpenHourDto updateStoreLocationTimeLineDto)
        {
            updateStoreLocationTimeLineDto.UserName = User.Identity.Name;

            var a = await _storeLocationServiceConnection.UpdateStoreLocationTimeLine(timeId, updateStoreLocationTimeLineDto);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/chi-tiet-cua-hang/" + id);
        }
        [Route("chi-tiet-cua-hang/{id}/ngon-ngu/{languageId}/xem-chi-tiet")]
        [HttpGet]
        public async Task<IActionResult> LanguageDetail(int id, Guid languageId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var store = await _storeLocationServiceConnection.GetStoreById(id);
            ViewBag.Name = store.Name;
            ViewBag.Id = id;
            ViewBag.LanguageId = languageId;
            var r = await _storeLocationServiceConnection.GetStoreLocationTranslationById(languageId);
            return View(r);
        }
        [Route("chi-tiet-cua-hang/{id}/them-moi-ngon-ngu")]
        [HttpGet]
        public async Task<IActionResult> CreateLanguage(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var store = await _storeLocationServiceConnection.GetStoreById(id);
            var language = await _languageServiceConnection.GetAllLanguages();
            ViewData["Language"] = new SelectList(language, "Id", "Name");
            ViewBag.Name = store.Name;
            ViewBag.Id = store.Id;
            return View();
        }


        [HttpPost]
        [Route("chi-tiet-cua-hang/{id}/them-moi-ngon-ngu")]

        public async Task<IActionResult> CreateLanguage(int id, CreateStoreLocationTranslationDto createStoreLocationTranslationDto)
        {
            createStoreLocationTranslationDto.StoreLocationId = id;
            createStoreLocationTranslationDto.UserName = User.Identity.Name;
            var a = await _storeLocationServiceConnection.CreateLanguage(createStoreLocationTranslationDto);

            if (a == true)
            {
                _notyfService.Success("Thêm mới thành công");
            }
            else
            {
                _notyfService.Error("Thêm mới thất bại");
            }

            return Redirect("/chi-tiet-cua-hang/" + id);
        }

        [Route("chi-tiet-cua-hang/{id}/ngon-ngu/{languageId}/cap-nhat")]
        [HttpGet]
        public async Task<IActionResult> UpdateLanguage(int id, Guid languageId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null || User.Claims.Where(x => x.Type == "Role").Select(x => x.Value).FirstOrDefault() != "AdminRole")
            {
                return Redirect("/dang-nhap.html");
            }

            var r = await _storeLocationServiceConnection.GetStoreLocationTranslationById(languageId);
            var store = await _storeLocationServiceConnection.GetStoreById(id);
            ViewBag.Name = store.Name;
            ViewBag.Id = id;
            ViewBag.LanguageId = languageId;

            UpdateStoreLocationTranslationDto updateStoreLocationTranslationDto = new()
            {
                Name = r.Name,
                Address = r.Address,
                City = r.City,
                
            };
            return View(updateStoreLocationTranslationDto);
        }


        [HttpPost]
        [Route("chi-tiet-cua-hang/{id}/ngon-ngu/{languageId}/cap-nhat")]

        public async Task<IActionResult> UpdateLanguage(int id, Guid languageId, UpdateStoreLocationTranslationDto updateStoreLocationTranslationDto)
        {
            updateStoreLocationTranslationDto.UserName = User.Identity.Name;

            var a = await _storeLocationServiceConnection.UpdateStoreLocationTranslation(languageId, updateStoreLocationTranslationDto);
            if (a == true)
            {
                _notyfService.Success("Cập nhật thành công");
            }
            else
            {
                _notyfService.Error("Cập nhật thất bại");
            }
            return Redirect("/chi-tiet-cua-hang/" + id);
        }
    }
}
