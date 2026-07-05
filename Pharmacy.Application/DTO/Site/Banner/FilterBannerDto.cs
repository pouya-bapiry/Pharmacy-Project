using Pharmacy.Domain.Entities.Site;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTO.Site.Banner
{
    public class FilterBannerDto
    {

        #region Properties
        public long Id { get; set; }


        //public long? UserId { get; set; }
        [Display(Name = "تصویر")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ImageName { get; set; }

        [Display(Name = "آدرس بنر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Url { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "سایز بنر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(25, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ColSize { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "تاریخ ویرایش")]
        public DateTime LastUpdateDate { get; set; }

        public string UserName { get; set; }
        public BannerPlacement Placement { get; set; }

        public bool IsDelete { get; set; }

        //  public List<SiteBanner> Banners{ get; set; }

        //public BannersLocations BannersLocations { get; set; }
        #endregion

        //public User User { get; set; }
   public enum BannerPlacement
    {
        First,
        Second,
        Third,
        Forth
    } 
    }
   
    public enum BannersLocation
    {
        Home1,

        Home2,

        Home3,

        Home4
    }

}


