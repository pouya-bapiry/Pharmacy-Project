using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTO.ProductCategory
{
    public class EditProductCategoryDto 
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }

        [Display(Name = "عنوان دسته بندی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "تصویر دسته بندی")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Image { get; set; }

        [Display(Name = "عنوان در لینک URL")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string UrlName { get; set; }

        [Display(Name = "آیکون")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Icon { get; set; }

        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }
    }

    public enum EditProductCategoryResult
    {
        NotFound,
        Error,
        Success,
        ImageErrorType,
    }
}
