namespace Pharmacy.Application.DTO.ProductOrder
{
    public class UserOrderDetailItemDto
    {
        #region Properties

        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public long? SellerId { get; set; }
        public string ProductTitle { get; set; }
        public string? ProductCode { get; set; }
        public int Count { get; set; }
        public long? ProductColorId { get; set; }
        public long? ProductSizeId { get; set; }
        public long? ProductSelectedId { get; set; }
        public int MainProductPrice { get; set; }
        public int ProductPrice { get; set; }
        public int OriginalProductPrice { get; set; }
        public int ProductColorPrice { get; set; }
        public int ProductSizePrice { get; set; }
        public int ProductShippingPrice { get; set; }
        public string ColorName { get; set; }
        public string SizeTitle { get; set; }
        public string ProductImage { get; set; }
        public string StoreName { get; set; }
        public int? DiscountPercentage { get; set; }
        public int DiscountPrice { get; set; }
      

        #endregion

    }
}
