using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tizhoshan.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CourseController : Controller
    {
        public CourseController()
        {

        }

        public IActionResult CourseList()
        {
            return View();
        }
    }
}
