using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.DTO.Site.Banner;
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
    #region Home Banner 1

    public class SiteBannerHome1ViewComponent : ViewComponent
    {
        private readonly ISiteImagesService _siteImagesService;

        public SiteBannerHome1ViewComponent(ISiteImagesService siteImagesService)
        {
            _siteImagesService = siteImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banners = await _siteImagesService.GetBannersByPlacement();
            return View("SiteBannerHome1", banners);
        }
    }

    #endregion

}
