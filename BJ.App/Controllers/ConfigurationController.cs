using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IConfiguration configuration;

        public ConfigurationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        [Route("/configuration")]
        public async Task<JsonResult> GetStore(string sectionName, string paramName)
        {
            var parameterValue = configuration[$"{sectionName}"];

            return Json(parameterValue);
        }
    }
}
