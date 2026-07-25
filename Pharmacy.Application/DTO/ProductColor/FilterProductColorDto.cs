using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.DTO.ProductColor
{
    public class FilterProductColorDto
    {
        #region Properties

        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        public int Price { get; set; }
        public string ProductName { get; set; }
        public string CreateDate { get; set; }

        #endregion
    }
}
