using Pharmacy.Application.DTO.ProductOrder;

namespace Pharmacy.Application.EntitiesExtensions
{
    public static class OrderExtensions
    {

        public static string GetTotalPriceWithDiscountForProduct(this UserOpenOrderDetailItemDto detail)
        {
            if (detail.DiscountPercentage != null)
            {
                return Convert.ToInt32(detail.Count * (detail.ProductPrice + Convert.ToInt32(detail.ProductColorPrice)) -
                                       (Convert.ToInt32(detail.Count * detail.DiscountPercentage *
                                           (detail.ProductPrice + Convert.ToInt32(detail.ProductColorPrice)) / 100)))
                    .ToString("#,0");
            }

            return (detail.Count * (detail.ProductPrice + Convert.ToInt32(detail.ProductColorPrice))).ToString("#,0");


        }

        public static string GetDiscountForProduct(this UserOpenOrderDetailItemDto detail)
        {
            if (detail.DiscountPercentage != null && detail.DiscountExpireDate >= DateTime.Now)
            {
                return (Convert.ToInt32(detail.Count * detail.DiscountPercentage *
                    (detail.ProductPrice + Convert.ToInt32(detail.ProductColorPrice)) / 100)).ToString("#,0");
            }

            return "- - -";

        }
    }
}
