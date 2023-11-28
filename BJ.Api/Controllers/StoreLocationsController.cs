using BJ.Application.Helper;
using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract;
using BJ.Contract.Size;
using BJ.Contract.StoreLocation;
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
        /// Danh sách size theo loại sản phẩm
        /// </summary>
        [HttpGet("{id}")]

        public async Task<IActionResult> GetStoresByCat(int id)
        {

            if (await _storeLocationService.GetStoreById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _storeLocationService.GetStoreById(id));
        }
        /// <summary>
        /// Danh sách cửa hàng
        /// </summary>
        [HttpGet]

        public async Task<IEnumerable<StoreLocationDto>> GetStoreLocations()
        {

            return await _storeLocationService.GetStoreLocation();

        }
        /// <summary>
        /// Thêm mới cửa hàng
        /// </summary>
        /// 
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]CreateStoreLocationDto createStoreLocationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                await _storeLocationService.CreateStoreLocation(createStoreLocationDto);

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
        public async Task<IActionResult> Edit(int id, [FromForm] UpdateStoreLocationDto updateStoreLocationDto)
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
                await _storeLocationService.UpdateStoreLocation(id, updateStoreLocationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

    }
}
