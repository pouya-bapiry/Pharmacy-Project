using System.ComponentModel.DataAnnotations;
using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Account;

namespace Pharmacy.Domain.Entities.ProductOrder
{
    public class BikeDelivery : BaseEntity
    {
        #region Properties

        public long UserId { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FullName { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "فقط اعداد مجاز می باشد")]
        public string Mobile { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "فقط اعداد مجاز می باشد")]
        public string NationalId { get; set; }

        [Display(Name = "تاریخ تولد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "تصویر مدارک وسیله نقلیه")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? VehicleDocumentImage { get; set; }

        [Display(Name = "تصویر خلافی وسیله نقلیه")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? VehicleViolationImage { get; set; }

        [Display(Name = "تصویر بیمه وسیله نقلیه")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? VehicleInsuranceImage { get; set; }

        [Display(Name = "تصویر گواهی نامه وسیله نقلیه")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? VehicleLicenseImage { get; set; }

        [Display(Name = "حالت پیک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public BikeDeliveryStatus BikeDeliveryStatus { get; set; }

        [Display(Name = "وضعیت پیک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public BikeDeliveryState BikeDeliveryState { get; set; }

        #endregion

        #region Relations

        public ICollection<Order> ProductOrders { get; set; }
        public User User { get; set; }

        #endregion
    }

    public enum BikeDeliveryStatus
    {
        [Display(Name = "در حال سفر")]
        Busy,
        
        [Display(Name = "در حال استراحت")]
        Rest,

        [Display(Name = "در حال آماده سازی")]
        Prepare,
    }

    public enum BikeDeliveryState
    {
        [Display(Name = "فعال")]
        Active,

        [Display(Name = "غیر فعال")]
        Deactive,

        [Display(Name = "معلق")]
        Suspend
    }
}
