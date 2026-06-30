

using Pharmacy.Application.DTO.Site.Slider;
using Pharmacy.Application.Utilities;

namespace Pharmacy.Application.EntitiesExtensions
{
    public static class SliderExtensions
    {
        public static string GetSliderImageAddress(this FilterSliderDto slider)
        {
            return PathExtension.SliderOrigin + slider.ImageName;
        }

        public static string GetMobileSliderImageAddress(this FilterSliderDto slider)
        {
            return PathExtension.MobileSliderOrigin + slider.MobileImageName;
        }

    }
}
