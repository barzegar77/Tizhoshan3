using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tizhoshan.ServiceLayer.DTOs.AccountViewModels;
using Tizhoshan.ServiceLayer.ENUMs.UserENUMs;
using Tizhoshan.ServiceLayer.Services.Interfaces;

namespace Tizhoshan.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                var user = _accountService.GetUserForLogin(model.PhoneNumber, model.Password);
                if (user == null)
                {
                    TempData["error"] = "اطلاعات ورود اشتباه است";
                }
                else if (user.PhoneNumberConfirmed == false)
                {
                    TempData["error"] = "شماره تلفن شما تایید نشده است";
                }
                else if (user.IsDeleted == true)
                {
                    TempData["error"] = "حساب کاربری شما مسدود است";
                }
                else
                {
                    var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                            new Claim(ClaimTypes.Name,user.PhoneNumber)
                        };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        AllowRefresh = true
                    };
                    await HttpContext.SignInAsync(principal, properties);
                    TempData["success"] = "ورود به حساب کاربری با موفقیت انجام شد";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Route("/SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                RegisterUserConditionsEnum res = _accountService.RegisterUser(model);

                if (res == RegisterUserConditionsEnum.AlternativePhone)
                {
                    TempData["error"] = "این شماره قبلا در سایت ثبت شده است";
                }

                if (res == RegisterUserConditionsEnum.OneMinuteNeeded)
                {
                    TempData["error"] = "بین هر درخواست باید یک دقیقه فاصله باشد";
                }

                if(res == RegisterUserConditionsEnum.Registerd)
                {
                    TempData["success"] = "ثبت نام شما با موفقیت ایجاد شد";
                    return RedirectToAction("ConfirmPhone", new { id = model.PhoneNumber });
                }
            }

            return View(model);

        }



        [Route("ConfirmPhone/{id}")]
        [HttpGet]
        public IActionResult ConfirmPhone(string id)
        {
            ViewBag.phoneNumber = id;
            return View();
        }

        [Route("ConfirmPhone")]
        [HttpPost]
        public async Task<IActionResult> ConfirmPhone(ConfirmPhoneViewModel model)
        {

            if (User.Identity.IsAuthenticated)
            {
                TempData["error"] = "حساب کاربری شما تائید شده است";
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {

                var res = _accountService.RegisterConfirmPhone(model);
                if (res == ConfirmPhoneRegisterEnum.Confirmed)
                {
                    //var user = _accountService.FindUserByPhoneNumber(model.PhoneNumber);
                    //var claims = new List<Claim>()
                    //{
                    //    new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                    //    new Claim(ClaimTypes.Name,user.PhoneNumber),
                    //    new Claim("id",user.UserId.ToString())
                    //};
                    //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //var principal = new ClaimsPrincipal(identity);

                    //var properties = new AuthenticationProperties
                    //{
                    //    IsPersistent = false
                    //};
                    //await HttpContext.SignInAsync(principal, properties);
                    TempData["success"] = "حساب کاربری شما با موفقیت ساخته شد";
                    return RedirectToAction("Index", "Home");
                }
                else if (res == ConfirmPhoneRegisterEnum.ErrorConfirmed)
                {
                    TempData["error"] = "کد تائید اشتباه است";
                }
                else if (res == ConfirmPhoneRegisterEnum.Expierd)
                {
                    TempData["error"] = "کد تائید منقضی شده , لطفا دوباره درخواست نمائید";
                }
                else
                {
                    TempData["error"] = "متاسفانه تائید شماره همراه موفقیت آمیز نبود , لطفا دوباره تلاش نمائید";
                }
            }

            return View(model);

        }





        [Route("RequestAnotherRegisterVerificationCode/{id}")]
        [HttpGet]
        public IActionResult RequestAnotherRegisterVerificationCode(string id)
        {
            string message = "";
            string status = "fail";
            if (User.Identity.IsAuthenticated)
            {
                return Json(new { status = "fail" });
            }
            var res = _accountService.RequestAnotherRegisterVerificationCode(id);//id is userName==phone number
            if (res == RequestAnotherRegisterVerificationCodeEnum.NotAllowed)
            {
                message = "بین هر درخواست باید 2 دقیقه فاصله باشد";
            }
            else if (res == RequestAnotherRegisterVerificationCodeEnum.UserNotFound)
            {
                message = "شماره همراه اشتباه است";
            }
            else if (res == RequestAnotherRegisterVerificationCodeEnum.UserIsActive)
            {
                message = "کاربر فعال امکان دریافت کد تایید را ندارد";

            }
            else if (res == RequestAnotherRegisterVerificationCodeEnum.NotSend)
            {
                message = "درخواست با خطا مواجه شد";
            }
            else
            {
                message = "کد تایید جدید برای " + id + " ارسال شد";
                status = "success";
            }
            return Json(new { status = status, message = message });
        }

        [Route("BaseChangePassword")]
        [HttpGet]
        public IActionResult BaseChangePassword()
        {
            return View();
        }

        //[Route("BaseChangePassword")]
        //[HttpPost]
        //public IActionResult BaseChangePassword(BaseChangePasswordViewModel model)
        //{
        //    string status = "";
        //    string message = "";

        //    if (!ModelState.IsValid)
        //    {
        //        var validationErrors = string.Join("<br />",
        //                           ModelState.Values.Where(E => E.Errors.Count > 0)
        //                           .SelectMany(E => E.Errors)
        //                           .Select(E => E.ErrorMessage)
        //                           .ToArray());
        //        return Json(new { status = "fail", message = validationErrors });
        //    }

        //    var res = _iAccountRepository.BaseChangePasswordSendSms(model);
        //    if (res == BaseChangePasswordSendSMSEnum.Error)
        //    {
        //        status = "fail";
        //        message = "متاسفانه ارسال کد تایید مقدور نمیباشد";
        //    }
        //    else if (res == BaseChangePasswordSendSMSEnum.NotAllowed)
        //    {
        //        status = "fail";
        //        message = "کاربر گرامی حساب کاربری شما غیر فعال است";
        //    }
        //    else if (res == BaseChangePasswordSendSMSEnum.UnconfirmedPhone)
        //    {
        //        status = "fail";
        //        message = "کاربر گرامی شماره همراه شما تایید نشده است لطفا مجدادا ثبت نام نمایید";
        //    }
        //    else if (res == BaseChangePasswordSendSMSEnum.OneMiniuteError)
        //    {
        //        status = "fail";
        //        message = "بین هر درخواست باید یک دقیقه فاصله باشد لطفا لحضاتی دیگر تلاش نمایید";
        //    }
        //    else
        //    {
        //        status = "success";
        //    }
        //    TempData["BaseEnterNewPasswordPhoneNumber"] = model.UserName;
        //    return Json(new { status = status, message = message });
        //}

        //[Route("BaseEnterNewPassword")]
        //[HttpGet]
        //public IActionResult BaseEnterNewPassword()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return Redirect("/Profile");
        //    }
        //    ViewBag.userName = TempData["BaseEnterNewPasswordPhoneNumber"].ToString();
        //    return PartialView("_BaseEnterNewPassword");
        //}

        //[Route("RequestAnotherChangePasswordVerificationCode/{id}")]
        //[HttpGet]
        //public IActionResult RequestAnotherChangePasswordVerificationCode(string id)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return Redirect("/Profile");
        //    }

        //    var res = _iAccountRepository.RequestAnotherChangePasswordVerificationCode(id);
        //    TempData["RequestAnotherChangePasswordVerificationFlag"] = res;
        //    if (res == RequestAnotherChangePasswordVerificationCodeEnum.Error)
        //    {
        //        TempData["BaseEnterNewPasswordPhoneNumber"] = id;
        //        return Redirect("/BaseEnterNewPassword");
        //    }
        //    if (res == RequestAnotherChangePasswordVerificationCodeEnum.NotAllowed)
        //    {
        //        TempData["BaseEnterNewPasswordPhoneNumber"] = id;
        //        return Redirect("/BaseEnterNewPassword");
        //    }
        //    if (res == RequestAnotherChangePasswordVerificationCodeEnum.UserNotFound)
        //    {
        //        return Redirect("/Login");
        //    }
        //    if (res == RequestAnotherChangePasswordVerificationCodeEnum.Sent)
        //    {
        //        TempData["BaseEnterNewPasswordPhoneNumber"] = id;
        //        return Redirect("/BaseEnterNewPassword");
        //    }

        //    TempData["BaseEnterNewPasswordPhoneNumber"] = id;
        //    return Redirect("/BaseEnterNewPassword");
        //}

        //[Route("BaseEnterNewPassword")]
        //[HttpPost]
        //public IActionResult BaseEnterNewPassword(BaseChangePasswordEnterNewViewModel model)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return Redirect("/Profile");
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        TempData["BaseEnterNewPasswordPhoneNumber"] = model.UserName;
        //        return View(model);
        //    }
        //    else
        //    {
        //        string status = "";
        //        string message = "";

        //        var res = _iAccountRepository.BaseChangePasswordFunction(model);
        //        if (res == BaseChangePasswordFunctionEnum.Error)
        //        {
        //            TempData["BaseEnterNewPasswordPhoneNumber"] = model.UserName;
        //            message = "متاسفانه تغییر رمز عبور موفقیت آمیز نبود";
        //            status = "fail";
        //        }
        //        if (res == BaseChangePasswordFunctionEnum.UserNotFound)
        //        {
        //            TempData["BaseEnterNewPasswordPhoneNumber"] = model.UserName;
        //            message = "متاسفانه تغییر رمز عبور موفقیت آمیز نبود";
        //            status = "fail";
        //        }
        //        if (res == BaseChangePasswordFunctionEnum.InvalidCode)
        //        {
        //            TempData["BaseEnterNewPasswordPhoneNumber"] = model.UserName;
        //            message = "کد تایید اشتباه است";
        //            status = "fail";
        //        }
        //        if (res == BaseChangePasswordFunctionEnum.CodeExspierd)
        //        {
        //            TempData["BaseEnterNewPasswordPhoneNumber"] = model.UserName;
        //            message = "کد تایید منقضی شده است";
        //            status = "fail";
        //        }
        //        if (res == BaseChangePasswordFunctionEnum.Changed)
        //        {
        //            TempData["BaseChangePassword"] = true;
        //            message = "رمز عبور شما با موفقیت تغییر یافت";
        //            status = "success";
        //        }
        //        return Json(new { status = status, message = message });
        //    }

        //}

    }
}
