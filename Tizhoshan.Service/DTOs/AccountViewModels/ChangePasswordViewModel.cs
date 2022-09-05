using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tizhoshan.ServiceLayer.DTOs.AccountViewModels
{
    public class ChangePasswordViewModel
    {
        public string PhoneNumber { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(15, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(4, ErrorMessage = "{0} نمیتواند کمتر از {1} کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(15, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(5, ErrorMessage = "{0} نمیتواند کمتر از {1} کاراکتر باشد")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "کلمه عبور و تکرار آن یکی نیست")]
        public string RePassword { get; set; }
    }
}
