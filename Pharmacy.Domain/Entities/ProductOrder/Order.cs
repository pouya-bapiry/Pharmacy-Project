using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;


namespace Pharmacy.Domain.Entities.ProductOrder
{
    public class Order : BaseEntity
    {
        #region Properties
        public long UserId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsPaid { get; set; }


        [Display(Name = "کد پیگیری تراکنش")]
        public string? TrackingCode { get; set; }

        [Display(Name = "کد پیگیری پرداخت")]
        public string? RefId { get; set; }

        [Display(Name = "مبلغ قابل پرداخت")]
        public string? OrderAmount { get; set; }

        [Display(Name = "مبلغ کل تخفیف")]
        public string? OrderDiscount { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        public OrderAcceptanceState OrderAcceptanceState { get; set; }

        #endregion

        #region Relations

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public UserAddress UserAddress { get; set; }

        #endregion

    }

    public enum OrderAcceptanceState
    {
        
        [Display(Name = "پرداخت شده")]
        PaymentSuccessful,

        [Display(Name = "لغو شده")]
        PaymentCancel,

        [Display(Name = "پرداخت نشده")]
        PaymentNotSuccessful,

        [Display(Name = "درحال بررسی")]
        UnderProgress
    }

}
