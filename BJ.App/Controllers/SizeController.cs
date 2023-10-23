using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers
{
    public class SizeController : Controller
    {
        private readonly ILogger<SizeController> _logger;
        private readonly ISizeServiceConnection _sizeService;

        public SizeController(ILogger<SizeController> logger, ISizeServiceConnection sizeService)
        {
            _logger = logger;
            _sizeService = sizeService;
        }


    }
}
