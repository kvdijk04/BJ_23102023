using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract.News;
using BJ.Contract.Translation.News;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewssController : ControllerBase
    {
        private readonly ILogger<NewssController> _logger;
        private readonly INewsService _blogService;
        public NewssController(ILogger<NewssController> logger, INewsService blogService)
        {
            _logger = logger;
            _blogService = blogService;
        }
        /// <summary>
        /// Phân trang tin tức
        /// </summary>
        [HttpGet("paging")]

        public async Task<PagedViewModel<NewsDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _blogService.GetPaging(getListPagingRequest);

        }
        /// <summary>
        /// Danh sách tin tức
        /// </summary>
        [HttpGet]

        public async Task<IEnumerable<NewsUserViewModel>> GetNewss(string culture, bool popular)
        {

            return await _blogService.GetNewss(culture, popular);

        }
        /// <summary>
        /// Danh sách tin tức tại trang chủ
        /// </summary>
        [HttpGet("homepage")]

        public async Task<IEnumerable<NewsUserViewModel>> GetNewsAtHome(string culture)
        {

            return await _blogService.GetNewsAtHome(culture);

        }
        /// <summary>
        /// Thêm mới tin tức
        /// </summary>
        /// 

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateNewsAdminView createNewsAdminView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _blogService.CreateNews(createNewsAdminView);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Thêm mới ngôn ngữ tin tức
        /// </summary>
        /// 
        [HttpPost("language")]
        public async Task<IActionResult> CreateLanguage([FromBody] CreateNewsTranslationDto createNewsTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _blogService.CreateNewsTranslate(createNewsTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin tin tức bằng id và ngôn ngữ
        /// </summary>

        [HttpGet("{id}/language/{culture}")]

        public async Task<IActionResult> GetNewsById(Guid id, string culture)
        {
            if (await _blogService.GetNewsById(id, culture) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _blogService.GetNewsById(id, culture));

        }

        /// <summary>
        /// Cập nhật tin tức bằng id
        /// </summary>

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateNews(Guid id, string culture, [FromForm] UpdateNewsAdminView updateNewsAdminView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _blogService.GetNewsById(id, culture) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                await _blogService.UpdateNews(id, updateNewsAdminView);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Lấy thông tin ngôn ngữ của tin tức bằng Id
        /// </summary>

        [HttpGet("language/{id}/detail")]

        public async Task<IActionResult> GetNewsTranslationById(Guid id)
        {
            if (await _blogService.GetNewsTransalationById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _blogService.GetNewsTransalationById(id));

        }

        /// <summary>
        /// Tạo mới tin tức theo từng ngôn ngữ
        /// </summary>
        /// 

        [HttpPost("language/create")]
        public async Task<IActionResult> CreateTranslate([FromBody] CreateNewsTranslationDto createNewsTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _blogService.CreateTranslateNews(createNewsTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Cập nhật tin tức theo từng ngôn ngữ
        /// </summary>
        /// 

        [HttpPut("language/{id}/update")]
        public async Task<IActionResult> UpdateTranslate(Guid id, [FromBody] UpdateNewsTranslationDto updateNewsTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _blogService.UpdateTranslateNews(id, updateNewsTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
