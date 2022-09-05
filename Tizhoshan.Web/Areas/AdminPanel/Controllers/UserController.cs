using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tizhoshan.ServiceLayer.DTOs.AccountViewModels;
using Tizhoshan.ServiceLayer.Services.Interfaces;

namespace Tizhoshan.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult UsersList(string searchString = "", int pageId = 1)
        {
            var users = _accountService.GetUsersListForAdmin(searchString, pageId);
            ViewBag.SearchString = searchString;
            return View(users);
        }


        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = _accountService.RegisterUserFromAdmin(model);
            if (userId == -100)
            {
                ModelState.AddModelError("PhoneNumber", "این شماره قبلا در سایت ثبت شده است");
                return View(model);
            }
            if (userId > 0)
            {
                TempData["success"] = "کاربر با موفقیت افزوده شد";

                return RedirectToAction("UsersList", "User");
            }
            TempData["error"] = "متاسفانه ثبت کاربر موفقیت آمیز نبود";

            return View(model);
        }
    }
}
