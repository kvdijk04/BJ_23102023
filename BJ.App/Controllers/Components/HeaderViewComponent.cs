using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IDetailConfigWebServiceConnection _detailConfigWebService;
        public HeaderViewComponent(IDetailConfigWebServiceConnection detailConfigWebService)
        {
            _detailConfigWebService = detailConfigWebService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string culture)
        {
            var all = await _detailConfigWebService.GetAllDetailConfigWebs(culture);

            all = all.Where(x => x.ConfigId == 1 && x.Active == true);

            return View(all);
        }
    }
}
