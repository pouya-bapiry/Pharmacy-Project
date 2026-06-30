using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.Services.Interfaces;

namespace Pharmacy.Web.ViewComponents
{
    #region Slider

    public class HomeSliderViewComponent : ViewComponent
    {
        private readonly ISiteImagesService _siteImagesService;

        public HomeSliderViewComponent(ISiteImagesService siteImagesService)
        {
            _siteImagesService = siteImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = await _siteImagesService.GetAllActiveSlider();
            return View("HomeSlider", sliders);
        }
    }

    #endregion
}
