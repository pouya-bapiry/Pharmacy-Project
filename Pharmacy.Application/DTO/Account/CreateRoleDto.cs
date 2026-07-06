using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTO.Account
{
    public class CreateRoleDto
    {

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string RoleName { get; set; }

    }

    public enum CreateRoleResult
    {
        Success,
        Error
    }
}
