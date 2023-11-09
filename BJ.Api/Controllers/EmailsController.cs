using BJ.Application.Email;
using BJ.Application.Helper;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly ILogger<EmailsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public EmailsController(ILogger<EmailsController> logger, IConfiguration configuration, IEmailSender emailSender)
        {
            _logger = logger;
            _configuration = configuration;
            _emailSender = emailSender;
        }
        [HttpPost]
        public IActionResult SendEmail([FromBody] FeedBack feedBack)
        {
            var emailTo = _configuration.GetValue<string>("EmailConfiguration:To");
            var message = Utilities.MailFeedBack(feedBack.Reason, feedBack.FullName, feedBack.Email, emailTo, feedBack.Phone, feedBack.VibeMember, feedBack.StoreName, feedBack.Message, DateTime.Now);
            _emailSender.SendEmail(message, feedBack.Email, feedBack.FullName);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
