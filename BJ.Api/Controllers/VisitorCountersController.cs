using BJ.Application.Service;
using BJ.Contract.VisitorCounter;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorCountersController : ControllerBase
    {
        private readonly ILogger<VisitorCountersController> _logger;
        private readonly IVisitorCounterService _visitorCounterService;
        public VisitorCountersController(ILogger<VisitorCountersController> logger, IVisitorCounterService visitorCounterService)
        {
            _logger = logger;
            _visitorCounterService = visitorCounterService;
        }

        /// <summary>
        /// Lượt truy cập
        /// </summary>
        [HttpGet]

        public async Task<VisitorCounterDto> GetVisitorCounters()
        {

            return await _visitorCounterService.GetVisitorCounter();

        }

        /// <summary>
        /// Cập nhật lượt truy cập
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateVisitorCounter([FromBody] UpdateVisitorCounterDto updateVisitorCounterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                await _visitorCounterService.UpdateCount(updateVisitorCounterDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
