using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;
        public CategoriesController(ILogger<CategoriesController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }
        /// <summary>
        /// Phân trang loại sản phẩm
        /// </summary>
        [HttpGet("paging")]

        public async Task<PagedViewModel<CategoryDto>> GetCategories([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _categoryService.GetPagingCategory(getListPagingRequest);

        }
        /// <summary>
        /// Danh sách loại
        /// </summary>
        [HttpGet]

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {

            return await _categoryService.GetCategoryDtos();

        }
        /// <summary>
        /// Danh sách loại trang user
        /// </summary>
        [HttpGet("userpage")]

        public async Task<IEnumerable<UserCategoryDto>> GetUserCategories()
        {

            return await _categoryService.GetUserCategoryDtos();

        }
        /// <summary>
        /// Tạo mới loại
        /// </summary>
        /// 
        [Authorize]

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateCategoryDto createCategoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _categoryService.CreateCategory(createCategoryDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Cập nhật loại
        /// </summary>
        /// 
        [Authorize]

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCategory(Guid id, [FromForm] UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _categoryService.GetCategoryById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                await _categoryService.UpdateCategory(id, updateCategoryDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// Lấy thông tin loại bằng Id
        /// </summary>

        [HttpGet("{id}")]

        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            if (await _categoryService.GetCategoryById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _categoryService.GetCategoryById(id));

        }

    }
}
