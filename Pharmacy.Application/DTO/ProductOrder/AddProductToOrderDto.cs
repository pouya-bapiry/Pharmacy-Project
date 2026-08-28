namespace Pharmacy.Application.DTO.ProductOrder
{
    public class AddProductToOrderDto
    {
        public long ProductId { get; set; }
        public int Count { get; set; }
        public long? ProductColorId { get; set; }
        public long? ProductSizeId { get; set; }
        public long? ProductSelectedId { get; set; }
       
    }
}
