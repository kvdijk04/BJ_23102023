using BJ.Application.Ultities;
using Microsoft.AspNetCore.Mvc;

namespace BJ.App.Controllers.Component
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagingBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
