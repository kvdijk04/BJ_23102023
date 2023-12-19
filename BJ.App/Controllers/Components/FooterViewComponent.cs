using BJ.ApiConnection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers.Components
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly IDetailConfigWebServiceConnection _detailConfigWebService;
        public FooterViewComponent(IDetailConfigWebServiceConnection detailConfigWebService)
        {
            _detailConfigWebService = detailConfigWebService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string culture)
        {
            var all = await _detailConfigWebService.GetAllDetailConfigWebs(culture);

            all = all.Where(x => x.Active == true);

            return View(all);
        }
    }
}
