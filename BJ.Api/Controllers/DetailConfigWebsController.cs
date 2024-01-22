using BJ.Application.Helper;
using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract;
using BJ.Contract.ConfigWeb;
using BJ.Contract.ConfigWeb.CreateConfigWeb;
using BJ.Contract.ConfigWeb.UpdateConfigWeb;
using BJ.Contract.Translation.ConfigWeb;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailConfigWebsController : ControllerBase
    {
        private readonly ILogger<DetailConfigWebsController> _logger;
        private readonly IDetailConfigWebService _detailConfigWebService;
        public DetailConfigWebsController(ILogger<DetailConfigWebsController> logger, IDetailConfigWebService detailConfigWebService)
        {
            _logger = logger;
            _detailConfigWebService = detailConfigWebService;
        }
        /// <summary>
        /// Phân trang chi tiết cấu hình web 
        /// </summary>
        //[SecurityRole(AuthorizeRole.AdminRole)]
        [HttpGet("paging")]

        public async Task<PagedViewModel<ConfigWebViewModel>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _detailConfigWebService.GetPaging(getListPagingRequest);

        }
        /// <summary>
        /// Danh sách chi tiết cấu hình web
        /// </summary>
        [HttpGet]

        public async Task<IEnumerable<ConfigWebViewModel>> GetDetailConfigWebs(string culture)
        {

            return await _detailConfigWebService.GetDetailConfigWebs(culture);

        }

        /// <summary>
        /// Thêm mới chi tiết cấu hình web
        /// </summary>
        /// 
        //[SecurityRole(AuthorizeRole.AdminRole)]

        [HttpPost]
        public async Task<string> Post(CreateConfigWebAdminView createConfigWebAdminView)
        {
            ApiResult apiResult = new();

            try
            {

                if (!ModelState.IsValid)
                {
                    apiResult.Msg = "Không có giá trị";
                    return apiResult.Msg;
                }
                var result = await _detailConfigWebService.CreateDetailConfigWeb(createConfigWebAdminView);

                return result.Msg;

            }
            catch (Exception e)
            {
                apiResult.Msg = e.Message;

                return apiResult.Msg;
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết cấu hình web bằng id
        /// </summary>

        [HttpGet("{id}")]

        public async Task<IActionResult> GetDetailConfigWebById(Guid id,string culture)
        {
            if (await _detailConfigWebService.GetDetailConfigWebById(id,culture) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _detailConfigWebService.GetDetailConfigWebById(id, culture));

        }
        /// <summary>
        /// Lấy thông tin chi tiết cấu hình web bằng địa chỉ url
        /// </summary>

        [HttpGet("url")]

        public async Task<IActionResult> GetDetailConfigWebByUrl(string url, string culture)
        {
            if (await _detailConfigWebService.GetDetailConfigWebByUrl(url, culture) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _detailConfigWebService.GetDetailConfigWebByUrl(url, culture));

        }
        /// <summary>
        /// Cập nhật chi tiết cấu hình web bằng id
        /// </summary>
        //[SecurityRole(AuthorizeRole.AdminRole)]

        [HttpPut("{id}")]

        public async Task<string> UpdateDetailConfigWeb(Guid id,string culture, [FromBody] UpdateDetailConfigWebDto updateDetailConfigWebDto)
        {
            ApiResult apiResult = new();

            try
            {

                if (!ModelState.IsValid)
                {
                    apiResult.Msg = "Không có giá trị";
                    return apiResult.Msg;
                }
                if (await _detailConfigWebService.GetDetailConfigWebById(id, culture) == null)
                {
                    apiResult.Msg = "Không tìm thấy cấu hình";
                    return apiResult.Msg;
                }

                var msg = await _detailConfigWebService.UpdateDetailConfigWeb(id, updateDetailConfigWebDto);


                return msg.Msg;
            }
            catch (Exception e)
            {
                apiResult.Msg = e.Message;

                return apiResult.Msg;
            }

        }

        /// <summary>
        /// Lấy thông tin ngôn ngữ của cấu hình trang bằng Id
        /// </summary>

        [HttpGet("language/{id}")]

        public async Task<IActionResult> GetDetailConfigWebTranslationById(Guid id)
        {
            if (await _detailConfigWebService.GetDetailConfigWebTranslationById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _detailConfigWebService.GetDetailConfigWebTranslationById(id));

        }

        /// <summary>
        /// Tạo mới cấu hình trang theo từng ngôn ngữ
        /// </summary>
        /// 
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.MarketingRole)]
        [HttpPost("language/create")]
        public async Task<IActionResult> CreateTranslate([FromBody] CreateDetailConfigWebTranslationDto createDetailConfigWebTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _detailConfigWebService.CreateTranslateDetailConfigWeb(createDetailConfigWebTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Cập nhật cấu hình trang theo từng ngôn ngữ
        /// </summary>
        /// 
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.MarketingRole)]

        [HttpPut("language/{id}")]
        public async Task<IActionResult> UpdateTranslate(Guid id, [FromBody] UpdateDetailConfigWebTranslationDto updateDetailConfigWebTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _detailConfigWebService.UpdateTranslateDetailConfigWeb(id, updateDetailConfigWebTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
