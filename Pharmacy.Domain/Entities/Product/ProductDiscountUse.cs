using Pharmacy.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domain.Entities.Product
{
    public class ProductDiscountUse : BaseEntity
    {
        #region Properties

        public long DiscountId { get; set; }

        #endregion

        #region Relations

        public ProductDiscount ProductDiscount { get; set; }

        #endregion
    }

}
