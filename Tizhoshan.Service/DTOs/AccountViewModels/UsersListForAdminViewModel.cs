using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tizhoshan.ServiceLayer.DTOs.AccountViewModels
{
    public class UsersListForAdminViewModel
    {
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string CreateDate { get; set; }
    }
}
