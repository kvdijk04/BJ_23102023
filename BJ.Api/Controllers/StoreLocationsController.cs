using BJ.Application.Service;
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
        public async Task<IActionResult> Post(CreateStoreLocationDto createStoreLocationDto)
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



    }
}
