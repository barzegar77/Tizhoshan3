#pragma checksum "D:\Sources\Tizhoshan\Tizhoshan\Tizhoshan.Web\Areas\UserPanel\Views\Shared\Components\SideBar\SideBar.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b63757c420258f11a59a79e514f12236d7ae3fd3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_UserPanel_Views_Shared_Components_SideBar_SideBar), @"mvc.1.0.view", @"/Areas/UserPanel/Views/Shared/Components/SideBar/SideBar.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Sources\Tizhoshan\Tizhoshan\Tizhoshan.Web\Areas\UserPanel\Views\_ViewImports.cshtml"
using Tizhoshan.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Sources\Tizhoshan\Tizhoshan\Tizhoshan.Web\Areas\UserPanel\Views\_ViewImports.cshtml"
using Tizhoshan.Web.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b63757c420258f11a59a79e514f12236d7ae3fd3", @"/Areas/UserPanel/Views/Shared/Components/SideBar/SideBar.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f2bda1454e3e94440c680eacceb41c0b5f9a8a0e", @"/Areas/UserPanel/Views/_ViewImports.cshtml")]
    public class Areas_UserPanel_Views_Shared_Components_SideBar_SideBar : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-area", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Account", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "SignOut", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<div class=\"col-lg-3 col-md-3\">\r\n    <div class=\"dashboard-navbar\">\r\n\r\n        <div class=\"d-user-avater\">\r\n            <img src=\"assets/img/user-3.jpg\" class=\"img-fluid avater\"");
            BeginWriteAttribute("alt", " alt=\"", 177, "\"", 183, 0);
            EndWriteAttribute();
            WriteLiteral(@">
            <h4>مهرداد محمدی</h4>
            <span>برنامه نویس ارشد</span>
            <div class=""elso_syu89"">
                <ul>
                    <li><a href=""#""><i class=""ti-facebook""></i></a></li>
                    <li><a href=""#""><i class=""ti-twitter""></i></a></li>
                    <li><a href=""#""><i class=""ti-instagram""></i></a></li>
                    <li><a href=""#""><i class=""ti-linkedin""></i></a></li>
                </ul>
            </div>
            <div class=""elso_syu77"">
                <div class=""one_third""><div class=""one_45ic text-warning bg-light-warning""><i class=""fas fa-star""></i></div><span>امتیازات</span></div>
                <div class=""one_third""><div class=""one_45ic text-success bg-light-success""><i class=""fas fa-file-invoice""></i></div><span>دوره ها</span></div>
                <div class=""one_third""><div class=""one_45ic text-purple bg-light-purple""><i class=""fas fa-user""></i></div><span>هنرجویان</span></div>
            </div>
        </div>

    ");
            WriteLiteral(@"    <div class=""d-navigation"">
            <ul id=""side-menu"">
                <li class=""active""><a href=""dashboard.html""><i class=""fas fa-th""></i>پیشخوان</a></li>
                <li class=""dropdown"">
                    <a href=""javascript:void(0);""><i class=""fas fa-shopping-basket""></i>دوره های آموزش<span class=""ti-angle-left""></span></a>
                    <ul class=""nav nav-second-level"">
                        <li><a href=""manage-course.html"">مدیریت</a></li>
                        <li><a href=""add-new-course.html"">افزودن دوره جدید</a></li>
                        <li><a href=""course-category.html"">دسته بندی دوره</a></li>
                        <li><a href=""coupons.html"">تخفیفات</a></li>
                    </ul>
                </li>
                <li class=""dropdown"">
                    <a href=""javascript:void(0);""><i class=""fas fa-gem""></i>ثبت نام ها<span class=""ti-angle-left""></span></a>
                    <ul class=""nav nav-second-level"">
                        <li><a href=");
            WriteLiteral(@"""enroll-history.html"">تاریخچه</a></li>
                        <li><a href=""enroll-student.html"">ثبت نام هنرجو</a></li>
                    </ul>
                </li>
                <li class=""dropdown"">
                    <a href=""javascript:void(0);""><i class=""fas fa-archive""></i>گزارشات<span class=""ti-angle-left""></span></a>
                    <ul class=""nav nav-second-level"">
                        <li><a href=""admin-revenue.html"">درآمد ادمین</a></li>
                        <li><a href=""instructor-revenue.html"">درآمد مربی</a></li>
                    </ul>
                </li>
                <li class=""dropdown"">
                    <a href=""javascript:void(0);""><i class=""fas fa-user-shield""></i>مدیرها<span class=""ti-angle-left""></span></a>
                    <ul class=""nav nav-second-level"">
                        <li><a href=""manage-admins.html"">مدیریت</a></li>
                        <li><a href=""add-admin.html"">افزودن مدیر جدید</a></li>
                    </ul>
            ");
            WriteLiteral(@"    </li>
                <li class=""dropdown"">
                    <a href=""javascript:void(0);""><i class=""fas fa-toolbox""></i>مدرسین<span class=""ti-angle-left""></span></a>
                    <ul class=""nav nav-second-level"">
                        <li><a href=""manage-instructor.html"">مدیریت</a></li>
                        <li><a href=""add-instructor.html"">ثبت مدرس جدید</a></li>
                        <li><a href=""instructor-payout.html"">پرداخت های مدرس</a></li>
                    </ul>
                </li>
                <li class=""dropdown"">
                    <a href=""javascript:void(0);""><i class=""fas fa-user""></i>هنرجویان<span class=""ti-angle-left""></span></a>
                    <ul class=""nav nav-second-level"">
                        <li><a href=""manage-students.html"">مدیریت</a></li>
                        <li><a href=""add-students.html"">ثبت هنرجو جدید</a></li>
                    </ul>
                </li>
                <li><a href=""addon-manager.html""><i class=""fas fa-la");
            WriteLiteral(@"yer-group""></i>افزونه ها</a></li>
                <li><a href=""themes.html""><i class=""fas fa-paint-brush""></i>قالب ها</a></li>
                <li><a href=""messages.html""><i class=""fas fa-comments""></i>پیام ها</a></li>
                <li><a href=""my-profile.html""><i class=""fas fa-address-card""></i>پروفایل کاربری</a></li>
                <li class=""dropdown"">
                    <a href=""javascript:void(0);""><i class=""fas fa-cog""></i>تنظیمات<span class=""ti-angle-left""></span></a>
                    <ul class=""nav nav-second-level"">
                        <li><a href=""website-settings.html"">وب سایت</a></li>
                        <li><a href=""system-settings.html"">سئو</a></li>
                        <li><a href=""payment_settings.html"">پرداخت</a></li>
                        <li><a href=""social-login.html"">شبکه های اجتماعی</a></li>
                        <li><a href=""smtp-setting.html"">ایمیل SMTP</a></li>
                        <li><a href=""dash-about.html"">درباره اپلیکیشن</a></li>
          ");
            WriteLiteral("          </ul>\r\n                </li>\r\n\r\n                <li>");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b63757c420258f11a59a79e514f12236d7ae3fd310035", async() => {
                WriteLiteral("<i class=\"fa fa-sign-out-alt\"></i>خروج از حساب");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Area = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("</li>\r\n            </ul>\r\n        </div>\r\n\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591