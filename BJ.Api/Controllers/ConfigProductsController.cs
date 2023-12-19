using BJ.Application.Helper;
using BJ.Application.Service;
using BJ.Contract;
using BJ.Contract.Size;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigProductsController : ControllerBase
    {
        private readonly ILogger<ConfigProductsController> _logger;
        private readonly IConfigProductService _configProductService;
        public ConfigProductsController(ILogger<ConfigProductsController> logger, IConfigProductService configProductService)
        {
            _logger = logger;
            _configProductService = configProductService;
        }
        /// <summary>
        /// Thêm mới dinh dưỡng của từng size cho từng sản phẩm
        /// </summary>
        /// 
        [SecurityRole(AuthorizeRole.AdminRole)]
        [HttpPost("sizespecific/nutrition")]
        public async Task<IActionResult> SizeSpecificProduct([FromBody] CreateSizeSpecificProductDto createSizeSpecificProduct)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _configProductService.CreateSizeSpecific(createSizeSpecificProduct);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Lấy thông tin size có sẵn cho từng sản phẩm
        /// </summary>
        [SecurityRole(AuthorizeRole.AdminRole)]

        [HttpGet("sizespecific/{id}/nutrition")]

        public async Task<IActionResult> GetSizeSpecificProductById(Guid id)
        {
            if (await _configProductService.GetSizeSpecificProductDtoById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _configProductService.GetSizeSpecificProductDtoById(id));

        }
        /// <summary>
        /// Cập nhật dinh dưỡng của từng size cho từng sản phẩm
        /// </summary>
        /// 
        [SecurityRole(AuthorizeRole.AdminRole)]
        [HttpPut("sizespecific/{id}/nutrition")]
        public async Task<IActionResult> SizeSpecificProduct(Guid id, [FromBody] UpdateSizeSpecificProductDto updateSizeSpecificProductDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _configProductService.GetSizeSpecificProductDtoById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                await _configProductService.UpdateSizeSpecific(id, updateSizeSpecificProductDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Điều chỉnh dinh dưỡng từng size và danh mục con cho từng sản phẩm
        /// </summary>
        /// 
        [SecurityRole(AuthorizeRole.AdminRole)]

        [HttpPost("product/subcategory/sizespecific/nutrition")]
        public async Task<IActionResult> EditSizeAndSubCat(ConfigProduct configProduct)
        {
            try
            {
                var a = await _configProductService.ConfifProduct(configProduct);

                if (a == true) return StatusCode(StatusCodes.Status200OK);

                return StatusCode(StatusCodes.Status400BadRequest);


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
