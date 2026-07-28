using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.DTO.Product
{
    public class EditProductDto
    {
        public long Id { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        //[Display(Name = "نام برند")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        //public string BrandName { get; set; }

        [Display(Name = "کد یا مدل محصول")]
        [MaxLength(350, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Code { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "فقط اعداد مجاز می باشد")]
        public int Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        public string? Description { get; set; }

        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }

        //[Display(Name = "پیام تایید / عدم تایید محصول")]
        //public string ProductAcceptOrRejectDescription { get; set; }

        [Display(Name = "تصویر محصول")]
        public string? ProductImage { get; set; }

    }

    public enum EditProductResult
    {
        NotFound,
        NotForUser,
        Success,
        Error,
        HasNoImage,
        ImageErrorType
    }
}
