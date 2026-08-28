
namespace Pharmacy.Application.DTO.ProductOrder
{
    public class UserOpenOrderDetailItemDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long SellerId { get; set; }
        public long? ProductColorId { get; set; }
        public long? ProductSizeId { get; set; }
        public long? ProductSelectedId { get; set; }
        public string ProductTitle { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductBrandName { get; set; }
        public int Count { get; set; }
        public int ProductPrice { get; set; }
        public int ProductSelectedPrice { get; set; }
        public int ProductColorPrice { get; set; }
        public int ProductSizePrice { get; set; }
        public int ProductShippingPrice { get; set; }
        public string ColorName { get; set; }
        public string SizeTitle { get; set; }
        public string ColorCode { get; set; }
        public string ProductImage { get; set; }
        public int? DiscountPercentage { get; set; }
        public int? DiscountNumber { get; set; }
        public int? DiscountUseNumber { get; set; }
        public DateTime? DiscountExpireDate { get; set; }
        public string SellerName { get; set; }
        public string StoreName { get; set; }
        public long SellerCode { get; set; }
        public string SellerAddress { get; set; }
        public long StockCount { get; set; }
        public bool IsInStock { get; set; }
    }
}
