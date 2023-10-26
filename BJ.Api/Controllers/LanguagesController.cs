using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly ILogger<LanguagesController> _logger;
        private readonly ILanguageService _sizeService;
        public LanguagesController(ILogger<LanguagesController> logger, ILanguageService sizeService)
        {
            _logger = logger;
            _sizeService = sizeService;
        }

        /// <summary>
        /// Danh sách ngôn ngữ
        /// </summary>
        [HttpGet]

        public async Task<IEnumerable<LanguageDto>> GetLanguages()
        {

            return await _sizeService.GetLanguages();

        }
        /// <summary>
        /// Thêm mới size
        /// </summary>
        /// 
        //[Authorize]

        //[HttpPost]
        //public async Task<IActionResult> Post(CreateLanguageDto createLanguageDto)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest();

        //        }
        //        await _sizeService.CreateLanguage(createLanguageDto);

        //        return StatusCode(StatusCodes.Status200OK);

        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        //    }
        //}


        
        /// <summary>
        /// Cập nhật size bằng id
        /// </summary>
        //[Authorize]

        //[HttpPut("{id}")]

        //public async Task<IActionResult> UpdateLanguage(int id, [FromBody] UpdateLanguageDto updateLanguageDto)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest();

        //        }
        //        if (await _sizeService.GetLanguageById(id) == null)
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound);
        //        }

        //        await _sizeService.UpdateLanguage(id, updateLanguageDto);

        //        return StatusCode(StatusCodes.Status200OK);

        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }

        //}
    }
}
