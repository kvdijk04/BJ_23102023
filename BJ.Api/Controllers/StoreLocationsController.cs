using BJ.Application.Helper;
using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract;
using BJ.Contract.StoreLocation;
using BJ.Contract.Translation.Store;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreLocationsController : ControllerBase
    {
        private readonly ILogger<StoreLocationsController> _logger;
        private readonly IStoreLocationService _storeLocationService;
        public StoreLocationsController(ILogger<StoreLocationsController> logger, IStoreLocationService storeLocationService)
        {
            _logger = logger;
            _storeLocationService = storeLocationService;
        }
        /// <summary>
        /// Phân trang size sản phẩm
        /// </summary>
        [HttpGet("paging")]
        public async Task<PagedViewModel<StoreLocationDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _storeLocationService.GetPaging(getListPagingRequest);

        }

        /// <summary>
        /// Chi tiết cửa hàng
        /// </summary>
        [HttpGet("{id}")]

        public async Task<IActionResult> GetStoresById(int id)
        {

            if (await _storeLocationService.GetStoreById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _storeLocationService.GetStoreById(id));
        }
                /// <summary>
        /// Chi tiết thời gian mở cửa cửa hàng
        /// </summary>
        [HttpGet("openinghours/{id}")]

        public async Task<IActionResult> GetStoresTimeLineById(int id)
        {

            if (await _storeLocationService.GetStoreLocationTimeLineById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _storeLocationService.GetStoreLocationTimeLineById(id));
        }
        /// <summary>
        /// Danh sách cửa hàng
        /// </summary>
        [HttpGet()]

        public async Task<IEnumerable<StoreLocationDto>> GetStoreLocations(string languageId)
        {

            return await _storeLocationService.GetStoreLocation(languageId);

        }
        /// <summary>
        /// Thêm mới cửa hàng
        /// </summary>
        /// 
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateStoreLocationAdminView createStoreLocationAdminView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                await _storeLocationService.CreateStoreLocation(createStoreLocationAdminView);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Thêm mới thời gian mở cửa hàng
        /// </summary>
        /// 
        [HttpPost("openinghours")]
        public async Task<IActionResult> CreateTimeLine([FromBody] CreateStoreLocationOpenHourDto createStoreLocationTimeLineDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                await _storeLocationService.CreateStoreLocationTimeline(createStoreLocationTimeLineDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Thêm mới ngôn ngữ
        /// </summary>
        /// 
        [HttpPost("language")]
        public async Task<IActionResult> CreateLanguage([FromBody] CreateStoreLocationTranslationDto createStoreLocationTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                await _storeLocationService.CreateTranslateStoreLocation(createStoreLocationTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Cập nhật cửa hàng
        /// </summary>
        /// 
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,[FromForm] UpdateStoreLocationAdminView updateStoreLocationAdminView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _storeLocationService.GetStoreById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                await _storeLocationService.UpdateStoreLocation(id, updateStoreLocationAdminView);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Cập nhật thời gian cửa hàng
        /// </summary>
        /// 
        [HttpPut("openinghours/{id}")]
        public async Task<IActionResult> EditTimeLine(int id,[FromBody] UpdateStoreLocationOpenHourDto updateStoreLocationTimeLineDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _storeLocationService.GetStoreLocationTimeLineById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                await _storeLocationService.UpdateStoreLocationTimeLine(id, updateStoreLocationTimeLineDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Cập nhật ngôn ngữ
        /// </summary>
        /// 
        [HttpPut("language/{id}")]
        public async Task<IActionResult> EditLanguage(Guid id, [FromBody] UpdateStoreLocationTranslationDto updateStoreLocationTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _storeLocationService.GetStoreLocationTranslatebyId(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                await _storeLocationService.UpdateTranslateStoreLocation(id,updateStoreLocationTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Lấy thông tin ngôn ngữ của loại bằng Id
        /// </summary>
        [SecurityRole(AuthorizeRole.AdminRole)]

        [HttpGet("language/{id}")]

        public async Task<IActionResult> GetStoreLocationTranslationById(Guid id)
        {
            if (await _storeLocationService.GetStoreLocationTranslatebyId(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _storeLocationService.GetStoreLocationTranslatebyId(id));

        }
    }
}
