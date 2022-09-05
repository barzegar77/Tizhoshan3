using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tizhoshan.Web.Areas.UserPanel.ViewComponents
{
    public class SideBarViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
           // var user = _

            return View("SideBar");
        }
    }
}
