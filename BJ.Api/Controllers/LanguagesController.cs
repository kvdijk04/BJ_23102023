using BJ.Application.Service;
using BJ.Contract.Translation;
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

    }
}
