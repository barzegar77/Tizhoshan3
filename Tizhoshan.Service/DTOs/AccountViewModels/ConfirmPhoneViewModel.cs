using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tizhoshan.ServiceLayer.DTOs.AccountViewModels
{
    public class ConfirmPhoneViewModel
    {

        [Required]
        public string PhoneNumber { get; set; }

 
        [Display(Name = "کد تایید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(5, ErrorMessage = "{0} باید 5 کاراکتر باشد", MinimumLength = 5)]
        public string Code { get; set; }
    }
}
