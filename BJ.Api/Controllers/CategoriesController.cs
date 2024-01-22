using BJ.Application.Helper;
using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract;
using BJ.Contract.Category;
using BJ.Contract.Translation.Category;
using BJ.Contract.ViewModel;
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
        [SecurityRole(AuthorizeRole.AdminRole)]
        [HttpGet("paging")]
        public async Task<PagedViewModel<CategoryDto>> GetCategories([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _categoryService.GetPagingCategory(getListPagingRequest);

        }
        /// <summary>
        /// Danh sách loại
        /// </summary>
        /// 
        [SecurityRole(AuthorizeRole.AdminRole)]
        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {

            return await _categoryService.GetCategoryDtos();

        }
        /// <summary>
        /// Danh sách loại trang user
        /// </summary>
        [SecurityRole(AuthorizeRole.AdminRole)]
        [HttpGet("userpage")]

        public async Task<IEnumerable<UserCategoryDto>> GetUserCategories(string languageId)
        {

            return await _categoryService.GetUserCategoryDtos(languageId);

        }
        /// <summary>
        /// Tạo mới loại
        /// </summary>
        /// 
        [SecurityRole(AuthorizeRole.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateCategoryAdminView createCategoryAdminView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _categoryService.CreateCategory(createCategoryAdminView);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Lấy thông tin ngôn ngữ của loại bằng Id
        /// </summary>
        [SecurityRole(AuthorizeRole.AdminRole)]

        [HttpGet("language/{id}")]

        public async Task<IActionResult> GetCategoryTranslationById(Guid id)
        {
            if (await _categoryService.GetCategoryTranslationById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _categoryService.GetCategoryTranslationById(id));

        }

        /// <summary>
        /// Tạo mới loại theo từng ngôn ngữ
        /// </summary>
        /// 
        [SecurityRole(AuthorizeRole.AdminRole)]
        [HttpPost("language")]

        public async Task<IActionResult> CreateTranslate([FromBody] CreateCategoryTranslationDto createCategoryTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _categoryService.CreateTranslateCategory(createCategoryTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Cập nhật loại theo từng ngôn ngữ
        /// </summary>
        /// 
        [SecurityRole(AuthorizeRole.AdminRole)]
        [HttpPut("language/{id}")]

        public async Task<IActionResult> UpdateTranslate( Guid id, [FromBody] UpdateCategoryTranslationDto updateCategoryTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _categoryService.UpdateTranslateCategory(id, updateCategoryTranslationDto);

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
        [SecurityRole(AuthorizeRole.AdminRole)]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCategory(Guid id, [FromForm] UpdateCategoryAdminView updateCategoryAdminView)
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

                await _categoryService.UpdateCategory(id, updateCategoryAdminView);

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
        [SecurityRole(AuthorizeRole.AdminRole)]
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
