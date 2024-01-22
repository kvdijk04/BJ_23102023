using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract.ConfigWeb;
using BJ.Contract.ConfigWeb.CreateConfigWeb;
using BJ.Contract.ConfigWeb.UpdateConfigWeb;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigWebsController : ControllerBase
    {
        private readonly ILogger<ConfigWebsController> _logger;
        private readonly IConfigWebService _configWebService;
        public ConfigWebsController(ILogger<ConfigWebsController> logger, IConfigWebService configWebService)
        {
            _logger = logger;
            _configWebService = configWebService;
        }
        /// <summary>
        /// Phân trang cấu hình web 
        /// </summary>
        //[SecurityRole(AuthorizeRole.AdminRole)]
        [HttpGet("paging")]

        public async Task<PagedViewModel<ConfigWebDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _configWebService.GetPaging(getListPagingRequest);

        }
        /// <summary>
        /// Danh sách cấu hình web
        /// </summary>
        [HttpGet]

        public async Task<IEnumerable<ConfigWebDto>> GetConfigWebs()
        {

            return await _configWebService.GetConfigWebs();

        }

        /// <summary>
        /// Thêm mới cấu hình web
        /// </summary>
        /// 
        //[SecurityRole(AuthorizeRole.AdminRole)]

        [HttpPost]
        public async Task<IActionResult> Post(CreateConfigWebDto createConfigWebDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _configWebService.CreateConfigWeb(createConfigWebDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin cấu hình web bằng id
        /// </summary>

        [HttpGet("{id}")]

        public async Task<IActionResult> GetConfigWebById(int id)
        {
            if (await _configWebService.GetConfigWebById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _configWebService.GetConfigWebById(id));

        }
        /// <summary>
        /// Cập nhật cấu hình web bằng id
        /// </summary>
        //[SecurityRole(AuthorizeRole.AdminRole)]

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateConfigWeb(int id, [FromBody] UpdateConfigWebDto updateConfigWebDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _configWebService.GetConfigWebById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                await _configWebService.UpdateConfigWeb(id, updateConfigWebDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
