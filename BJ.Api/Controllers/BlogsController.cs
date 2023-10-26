using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract.Blog;
using BJ.Contract.Translation.Blog;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly ILogger<BlogsController> _logger;
        private readonly IBlogService _blogService;
        public BlogsController(ILogger<BlogsController> logger, IBlogService blogService)
        {
            _logger = logger;
            _blogService = blogService;
        }
        /// <summary>
        /// Phân trang blog sản phẩm
        /// </summary>
        [HttpGet("paging")]

        public async Task<PagedViewModel<BlogDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _blogService.GetPaging(getListPagingRequest);

        }
        /// <summary>
        /// Danh sách blog
        /// </summary>
        [HttpGet]

        public async Task<IEnumerable<BlogUserViewModel>> GetBlogs(string culture, bool popular)
        {

            return await _blogService.GetBlogs(culture, popular);

        }
        /// <summary>
        /// Thêm mới blog
        /// </summary>
        /// 

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateBlogAdminView createBlogAdminView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _blogService.CreateBlog(createBlogAdminView);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Thêm mới ngôn ngữ blog
        /// </summary>
        /// 
        [HttpPost("language")]
        public async Task<IActionResult> CreateLanguage([FromBody] CreateBlogTranslationDto createBlogTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _blogService.CreateBlogTranslate(createBlogTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin blog bằng id
        /// </summary>

        [HttpGet("{id}")]

        public async Task<IActionResult> GetBlogById(Guid id, string culture)
        {
            if (await _blogService.GetBlogById(id, culture) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _blogService.GetBlogById(id, culture));

        }
        /// <summary>
        /// Cập nhật blog bằng id
        /// </summary>
        [Authorize]

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateBlog(Guid id, string culture, [FromBody] UpdateBlogDto updateBlogDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _blogService.GetBlogById(id, culture) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                await _blogService.UpdateBlog(id, updateBlogDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
