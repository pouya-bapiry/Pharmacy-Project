using Pharmacy.Application.DTO.ProductOrder;

namespace Pharmacy.Application.DTO.ProductOrder
{
    public class UserOpenOrderDto
    {
        public long UserId { get; set; }
        public string Description { get; set; }
        public List<UserOpenOrderDetailItemDto> Details { get; set; }

        public decimal GetTotalPriceWithoutDiscount()
        {
            return Details.Sum(x => (decimal)x.Count * x.ProductPrice);
        }

        public decimal GetTotalDiscountPrice()
        {
            return (decimal)Details.Sum(x =>
                ((decimal)x.Count * x.DiscountPercentage * x.ProductPrice) / 100m
            );
        }

        public decimal GetTotalShippingPrice()
        {
            return Details.Sum(x =>
                (decimal)x.ProductShippingPrice * x.Count
            );
        }

        public decimal GetTotalPriceWithFreeShipping()
        {
            return GetTotalPriceWithDiscount() - GetTotalShippingPrice();
        }

        public decimal GetTotalPriceWithDiscount()
        {
            return GetTotalPriceWithoutDiscount() - GetTotalDiscountPrice();
        }

    }

}
