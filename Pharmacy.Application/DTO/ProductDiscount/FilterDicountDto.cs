using Pharmacy.Application.DTO.Paging;

namespace Pharmacy.Application.DTO.ProductDiscount
{
    public class FilterDiscountDto : BasePaging
    {
        #region Properties

        public long? ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int Percentage { get; set; }
        public DateTime ExpireDate { get; set; }
        public int? DiscountNumber { get; set; }
        public string CreateDate { get; set; }
        public List<Domain.Entities.Product.ProductDiscount> ProductDiscounts { get; set; }

        #endregion

        #region Methods

        public FilterDiscountDto SetProductDiscount(List<Domain.Entities.Product.ProductDiscount> productDiscounts)
        {
            this.ProductDiscounts = productDiscounts;
            return this;
        }

        public FilterDiscountDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }

   

}
