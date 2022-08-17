using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tizhoshan.DataLayer.Models.Account
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string DisplayName { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Password { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; } = false;

        public string ConfirmationCode { get; set; }
        public DateTime ConfirmationCodeDateTime { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; } = false;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        [Display(Name = "آواتار")]
        public string AvatarName { get; set; }


    }
}
