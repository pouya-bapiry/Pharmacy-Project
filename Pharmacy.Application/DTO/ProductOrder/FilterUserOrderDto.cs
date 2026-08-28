using Pharmacy.Application.DTO.Paging;
using Pharmacy.Domain.Entities.ProductOrder;
using System.ComponentModel.DataAnnotations;


namespace Pharmacy.Application.DTO.ProductOrder
{
    public class FilterUserOrderDto : BasePaging
    {
        #region Constructor

        public FilterUserOrderDto()
        {
            OrderBy = FilterUserOrder.CreateDateDescending;
        }

        #endregion

        #region Properties

        public long? UserId { get; set; }
        public long? BikeDeliveryId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsPaid { get; set; }
        

        [Display(Name = "کد پیگیری")]
        public string TrackingCode { get; set; }

        [Display(Name = "کد پیگیری پرداخت")]
        public string RefId { get; set; }

        [Display(Name = "مبلغ پرداخت شده")]
        public string? OrderAmount { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        public FilterUserOrderState FilterUserOrderState { get; set; }
        public FilterUserOrder OrderBy { get; set; }
        public FilterPaymentMethod FilterPaymentMethod { get; set; }
        public FilterOrderPeriodTime FilterOrderPeriodTime { get; set; }
        public FilterOrderDelivered FilterOrderDelivered { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        #endregion

        #region Methods

        public FilterUserOrderDto SetUserOrders(List<Order> orders)
        {
            this.Orders = orders;
            return this;
        }

        public FilterUserOrderDto SetUserOrderDetails(List<OrderDetail> orderDetails)
        {
            this.OrderDetails = orderDetails;
            return this;
        }

        public FilterUserOrderDto SetPaging(BasePaging paging)
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

    public enum FilterUserOrderState
    {
        [Display(Name = "همه")]
        All,

        [Display(Name = "پرداخت شده")]
        PaymentSuccessful,

        [Display(Name = "پرداخت نشده")]
        PaymentNotSuccessful,

        [Display(Name = "لغو شده")]
        PaymentCancel,

        [Display(Name = "درحال بررسی")]
        UnderProgress
    }

    public enum FilterPaymentMethod
    {
        [Display(Name = "همه")]
        All,

        [Display(Name = "آنلاین (از طریق درگاه بانکی)")]
        BankPayment,

        [Display(Name = "درب منزل (درحال بررسی)")]
        HomePaymentUnderProgress,

        [Display(Name = "درب منزل (پرداخت شده)")]
        HomePaymentSuccessful
    }

    public enum FilterOrderPeriodTime
    {
        [Display(Name = "همه")]
        All,

        [Display(Name = "8 تا 11 صبح")]
        PartOne,

        [Display(Name = "12 تا 15 ظهر")]
        PartTwo,

        [Display(Name = "16 تا 19 عصر")]
        PartThree,

        [Display(Name = "20 تا 23 شب")]
        PartFour,
    }

    public enum FilterUserOrder
    {
        CreateDateDescending,
        CreateDateAscending,

    }

    public enum FilterOrderDelivered
    {
        [Display(Name = "همه")]
        All,

        [Display(Name = "تحویل گردیده است")]
        Delivered,

        [Display(Name = "تحویل نگردیده است")]
        NotDelivered,
    }
}
