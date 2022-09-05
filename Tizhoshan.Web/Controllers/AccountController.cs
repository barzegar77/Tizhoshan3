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
        public IActionResult ConfirmPhone(ConfirmPhoneViewModel model)
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
                    TempData["success"] = "حساب کاربری شما با موفقیت ساخته شد";
                    return RedirectToAction("Login");
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
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Profile");
            }

            var res = _accountService.RequestAnotherChangePasswordVerificationCode(id);

            if (res == RequestAnotherChangePasswordVerificationCodeEnum.Error)
            {
                TempData["error"] = "خطا در ارسال کد تایید";
                return RedirectToAction("ConfirmPhone", new { id = id });
            }
            if (res == RequestAnotherChangePasswordVerificationCodeEnum.NotAllowed)
            {
                TempData["error"] = "کاربر گرامی لطفا بعد از 1 دقیقه دوباره تلاش کنید";
                return RedirectToAction("ConfirmPhone", new { id = id });
            }
            if (res == RequestAnotherChangePasswordVerificationCodeEnum.UserNotFound)
            {
                TempData["error"] = "کاربری یافت نشد";
                return RedirectToAction("Login");
            }
            if (res == RequestAnotherChangePasswordVerificationCodeEnum.Sent)
            {
                TempData["success"] = "کاربر گرامی کد تایید مجدد ارسال شد";
                return RedirectToAction("ConfirmPhone", new { id = id });
            }

            TempData["error"] = "خطا در ارسال کد تایید";
            return RedirectToAction("ConfirmPhone", new { id = id });
        }


        [Route("ForgotPassword")]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var res = _accountService.ForgotPasswordSendSms(model);
                if (res == ForgotPasswordSendSMSEnum.Error)
                {
                    TempData["error"] = "متاسفانه ارسال کد تایید مقدور نمیباشد";
                }
                else if (res == ForgotPasswordSendSMSEnum.NotAllowed)
                {
                    TempData["error"] = "کاربر گرامی حساب کاربری شما غیر فعال است لطفا مجدد ثبت نام کنید";
                }
                else if (res == ForgotPasswordSendSMSEnum.UnconfirmedPhone)
                {
                    TempData["error"] = "کاربر گرامی شماره همراه شما تایید نشده است لطفا مجدد ثبت نام نمایید";
                }
                else if (res == ForgotPasswordSendSMSEnum.OneMiniuteError)
                {
                    TempData["error"] = "بین هر درخواست باید یک دقیقه فاصله باشد لطفا لحضاتی دیگر تلاش نمایید";
                }

                if(res == ForgotPasswordSendSMSEnum.Sent)
                {
                    TempData["ChangePasswordPhoneNumber"] = model.PhoneNumber;
                    return RedirectToAction("ConfirmForgotPasswordPhone");
                }
            }

            return View(model);
        }

        [Route("ConfirmForgotPasswordPhone")]
        [HttpGet]
        public IActionResult ConfirmForgotPasswordPhone()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["error"] = "محدودیت دسترسی";
                return Redirect("/Home/Index");
            }
            ViewBag.PhoneNumber = TempData["ChangePasswordPhoneNumber"].ToString();
            return View();
        }


        [Route("ConfirmForgotPasswordPhone")]
        [HttpPost]
        public IActionResult ConfirmForgotPasswordPhone(ConfirmPhoneViewModel model)
        {
            if (ModelState.IsValid)
            {

                var res = _accountService.RegisterConfirmPhone(model);
                if (res == ConfirmPhoneRegisterEnum.Confirmed)
                {
                    return RedirectToAction("ChangePassword", new { phoneNumber = model.PhoneNumber});
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


            ViewBag.PhoneNumber = model.PhoneNumber;

            return View(model);

        }


        [Route("ChangePassword/{phoneNumber}")]
        [HttpGet]
        public IActionResult ChangePassword(string phoneNumber)
        {
            ViewBag.PhoneNumber = phoneNumber;
            return View();
        }



        [Route("ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Profile");
            }
            if (ModelState.IsValid)
            {
                var res = _accountService.ChangePassword(model);
                if (res == true)
                {
                    TempData["success"] = "کاربر گرامی رمز عبور شما با موفقیت تغییر یافت";
                    return RedirectToAction("Login");
                }
            }

            return View(model);

        }


        [Route("RequestAnotherChangePasswordVerificationCode/{id}")]
        [HttpGet]
        public IActionResult RequestAnotherChangePasswordVerificationCode(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Profile");
            }

            var res = _accountService.RequestAnotherChangePasswordVerificationCode(id);

            if(id != null)
            {
                TempData["ChangePasswordPhoneNumber"] = id;
            }

            if (res == RequestAnotherChangePasswordVerificationCodeEnum.Error)
            {
                TempData["error"] = "خطا در ارسال کد تایید";
                return RedirectToAction("ConfirmForgotPasswordPhone");
            }
            if (res == RequestAnotherChangePasswordVerificationCodeEnum.NotAllowed)
            {
                TempData["error"] = "کاربر گرامی لطفا بعد از 1 دقیقه دوباره تلاش کنید";
                return RedirectToAction("ConfirmForgotPasswordPhone");
            }
            if (res == RequestAnotherChangePasswordVerificationCodeEnum.UserNotFound)
            {
                TempData["error"] = "کاربری یافت نشد";
                return RedirectToAction("Login");
            }
            if (res == RequestAnotherChangePasswordVerificationCodeEnum.Sent)
            {
                TempData["success"] = "کاربر گرامی کد تایید مجدد ارسال شد";
                return RedirectToAction("ConfirmForgotPasswordPhone");
            }

            TempData["error"] = "خطا در ارسال کد تایید";
            return RedirectToAction("ConfirmForgotPasswordPhone");
        }



    }
}
