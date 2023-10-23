using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract.Size;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly ILogger<SizesController> _logger;
        private readonly ISizeService _sizeService;
        public SizesController(ILogger<SizesController> logger, ISizeService sizeService)
        {
            _logger = logger;
            _sizeService = sizeService;
        }
        /// <summary>
        /// Phân trang size sản phẩm
        /// </summary>
        [HttpGet("paging")]

        public async Task<PagedViewModel<SizeDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _sizeService.GetPaging(getListPagingRequest);

        }
        /// <summary>
        /// Danh sách size
        /// </summary>
        [HttpGet]

        public async Task<IEnumerable<SizeDto>> GetSizes()
        {

            return await _sizeService.GetSizes();

        }
        /// <summary>
        /// Thêm mới size
        /// </summary>
        /// 
        [Authorize]

        [HttpPost]
        public async Task<IActionResult> Post(CreateSizeDto createSizeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _sizeService.CreateSize(createSizeDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        /// <summary>
        /// Lấy thông tin size từng sản phẩm
        /// </summary>

        [HttpGet("{sizeId}/{productId}")]

        public async Task<IActionResult> SizeSpecificProductById(Guid productId, int sizeId)
        {
            if (await _sizeService.GetSize(sizeId, productId) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _sizeService.GetSize(sizeId, productId));

        }
        /// <summary>
        /// Lấy thông tin size bằng id
        /// </summary>

        [HttpGet("{id}")]

        public async Task<IActionResult> GetSizeById(int id)
        {
            if (await _sizeService.GetSizeById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _sizeService.GetSizeById(id));

        }
        /// <summary>
        /// Cập nhật size bằng id
        /// </summary>
        [Authorize]

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateSize(int id, [FromBody] UpdateSizeDto updateSizeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _sizeService.GetSizeById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                await _sizeService.UpdateSize(id, updateSizeDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
