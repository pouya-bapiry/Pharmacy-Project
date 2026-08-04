namespace Pharmacy.Application.DTO.Product
{
    public class ProductDetailsDto
    {
        public long ProductId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int? View { get; set; }
        public List<Domain.Entities.Product.ProductGallery> ProductGalleries { get; set; }
        public List<Domain.Entities.Product.ProductColor> ProductColors { get; set; }
        public List<Domain.Entities.Product.ProductCategory> ProductCategories { get; set; }
        public List<Domain.Entities.Product.ProductFeature> ProductFeatures { get; set; }
       public List<Domain.Entities.Product.Product> RelatedProducts { get; set; }
       // public List<Domain.Entities.Product> ProductComments { get; set; }
       public Domain.Entities.Product.ProductDiscount ProductDiscount { get; set; }
       // public ProductBrand ProductBrand { get; set; }
    }
}
