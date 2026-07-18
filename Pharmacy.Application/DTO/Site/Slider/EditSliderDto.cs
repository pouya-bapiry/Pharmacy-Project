using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTO.Site.Slider
{
    public class EditSliderDto
    {

        #region Properties
        public long Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "لینک")]
        public string Link { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

       
        [Display(Name = "نام تصویر")]
     
        public string? ImageName { get; set; }

       
        [Display(Name = "نام تصویر موبایل")]
       
        public string? MobileImageName { get; set; }

        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }


        #endregion
    }

    public enum EditSliderResult
    {
        Success,
        Error,
        NotFound,
    }
}
