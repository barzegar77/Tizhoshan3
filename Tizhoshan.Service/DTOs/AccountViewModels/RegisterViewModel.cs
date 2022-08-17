using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tizhoshan.ServiceLayer.DTOs.AccountViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(25, ErrorMessage = "{0} باید بین {2} کاراکتر تا {1} کاراکتر باشد", MinimumLength = 6)]
        public string DisplayName { get; set; }


        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(11, ErrorMessage = "{0} باید 11 کاراکتر باشد.", MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(20, ErrorMessage = "{0} باید بین {2} کاراکتر تا {1} کاراکتر باشد", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا {0} را وارد نمایید")]
        [StringLength(20, ErrorMessage = "{0} باید بین {2} کاراکتر تا {1} کاراکتر باشد", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "کلمه عبور و تکرار آن یکی نیست")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }

    }
}
