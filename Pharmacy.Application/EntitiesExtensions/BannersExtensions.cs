using Pharmacy.Application.DTO.Site.Banner;
using Pharmacy.Application.Utilities;

namespace Pharmacy.Application.EntitiesExtensions
{
    public static class BannersExtensions
    {
        public static string GetSiteMainImageAddress(this FilterBannerDto banner)
        {
            return PathExtension.BannerOrigin + banner.ImageName;
        }
    }
}
