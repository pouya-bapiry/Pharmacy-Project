using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.DTO.ProductGallery
{
    public class EditProductGallery
    {
        public long Id { get; set; }
        public long ProductId { get; set; }

        [Display(Name = "الویت نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int DisplayPriority { get; set; }

        [Display(Name = "تصویر گالری")]
       
        public string? ImageName { get; set; }

    }

    public enum EditProductGalleryResult
    {
        Success,
        Error,
        NotForUserProduct,
        ImageIsNull,
        ProductNotFound
    }
}

