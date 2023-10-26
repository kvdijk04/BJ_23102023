using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract.Product;
using BJ.Contract.Translation.Product;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        /// <summary>
        /// Danh sách sản phẩm
        /// </summary>
        [HttpGet]

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {

            return await _productService.GetProductDtos();

        }
        /// <summary>
        /// Danh sách sản phẩm web user
        /// </summary>
        [HttpGet("userpage")]

        public async Task<ProductUserViewModel> GetClientProducts(string languageId)
        {

            return await _productService.GetProduct(languageId);

        }
        /// <summary>
        /// Phân trang sản phẩm
        /// </summary>
        [HttpGet("paging")]

        public async Task<PagedViewModel<ViewAllProduct>> GetProducts([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _productService.GetPaging(getListPagingRequest);

        }
        /// <summary>
        /// Thêm mới sản phẩm
        /// </summary>
        /// 
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateProductAdminView createProductAdminView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _productService.CreateProductAdminView(createProductAdminView);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Thêm mới ngôn ngữ cho từng sản phẩm
        /// </summary>
        /// 
        [Authorize]
        [HttpPost("language")]
        public async Task<IActionResult> CreateLanguage([FromBody] CreateProductTranslationDto createProductTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _productService.CreateProductTranslate(createProductTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Lấy thông tin ngôn ngữ sản phẩm bằng Id
        /// </summary>

        [HttpGet("language/{id}/detail")]

        public async Task<IActionResult> GetProductTranslationById(Guid id)
        {
            if (await _productService.GetProductTranslationDto(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _productService.GetProductTranslationDto(id));

        }
        /// <summary>
        /// Cập nhật ngôn ngữ sản phẩm
        /// </summary>
        [Authorize]

        [HttpPut("{proId}/language/{id}/update")]
        public async Task<IActionResult> EditProduct(Guid proId, Guid id, [FromBody] UpdateProductTranslationDto updateProductTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _productService.GetProductTranslationDto(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                await _productService.UpdateProductTranslate(proId, id, updateProductTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Cập nhật sản phẩm
        /// </summary>
        [Authorize]

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(Guid id, [FromForm] UpdateProductAdminView updateProductAdminView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _productService.GetProductById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                await _productService.UpdateProductAdminView(id, updateProductAdminView);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Lấy thông tin sản phẩm bằng Id
        /// </summary>

        [HttpGet("{id}")]

        public async Task<IActionResult> GetProductById(Guid id)
        {
            if (await _productService.GetProductById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _productService.GetProductById(id));

        }
        /// <summary>
        /// Lấy thông tin sản phẩm bằng Id web user
        /// </summary>

        [HttpGet("userpage/{id}/{languageId}")]

        public async Task<IActionResult> GetUserProductById(Guid id, string languageId)
        {
            if (await _productService.GetUserProductById(id, languageId) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _productService.GetUserProductById(id, languageId));

        }
        /// <summary>
        /// Lấy thông tin sản phẩm bằng Id loại
        /// </summary>

        [HttpGet("category/filter")]

        public async Task<IActionResult> GetProductByCatId(Guid catId, bool popular, string languageId)
        {
            if (await _productService.GetProductByCatId(catId, popular, languageId) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _productService.GetProductByCatId(catId, popular, languageId));

        }
    }
}
