using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tizhoshan.ServiceLayer.DTOs.AccountViewModels
{
    public class BaseChangePasswordViewModel
    {
        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(11, ErrorMessage = "{0} یک عدد 11 رقمی است")]
        [MinLength(11, ErrorMessage = "{0} یک عدد 11 رقمی است")]
        [RegularExpression(@"[0-9\s]+", ErrorMessage = "لطفا فقط عدد وارد نمایید")]
        public string PhoneNumber { get; set; }
    }
}
