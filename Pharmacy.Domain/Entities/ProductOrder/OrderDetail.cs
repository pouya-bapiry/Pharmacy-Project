
using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Product;

namespace Pharmacy.Domain.Entities.ProductOrder
{
    public class OrderDetail : BaseEntity
    {
        #region Properties

        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Count { get; set; }
        public int ProductPrice { get; set; }


        #endregion

        #region Relations

        public Order Order { get; set; }
        public Product.Product Product { get; set; }

        #endregion
    }
}
