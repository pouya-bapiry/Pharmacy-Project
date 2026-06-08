using Pharmacy.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domain.Entities.Account
{
    public class Role : BaseEntity
    {

        [Display(Name = "نام نقش")]
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمیتواند بیشتر از {1} کارکتر باشد")]
        public string RoleName { get; set; }

        #region Relations

        public ICollection<User> Users { get; set; }

        #endregion
    }
}
