using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tizhoshan.ServiceLayer.DTOs.AccountViewModels
{
    public class UserInfoViewModel
    {
        public string DisplayName { get; set; }
        public string CreateDate { get; set; }
        public string AvatarName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
