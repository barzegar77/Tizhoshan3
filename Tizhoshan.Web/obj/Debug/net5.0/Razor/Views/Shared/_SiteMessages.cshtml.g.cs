#pragma checksum "D:\Sources\Tizhoshan\Tizhoshan\Tizhoshan.Web\Views\Shared\_SiteMessages.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "148b4984c467cf477218664c2aa44509bf49713f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__SiteMessages), @"mvc.1.0.view", @"/Views/Shared/_SiteMessages.cshtml")]
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
#line 1 "D:\Sources\Tizhoshan\Tizhoshan\Tizhoshan.Web\Views\_ViewImports.cshtml"
using Tizhoshan.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Sources\Tizhoshan\Tizhoshan\Tizhoshan.Web\Views\_ViewImports.cshtml"
using Tizhoshan.Web.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"148b4984c467cf477218664c2aa44509bf49713f", @"/Views/Shared/_SiteMessages.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f2bda1454e3e94440c680eacceb41c0b5f9a8a0e", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__SiteMessages : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\Sources\Tizhoshan\Tizhoshan\Tizhoshan.Web\Views\Shared\_SiteMessages.cshtml"
 if (
TempData["success"] != null ||
TempData["error"] != null ||
TempData["info"] != null ||
TempData["warning"] != null
)
{
    if (TempData["success"] != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        <script>
            let message = $(""#showMessage1"").val();
            Toastify({
                text: message,
                duration: 3000,
                newWindow: true,
                close: false,
                gravity: ""top"", // `top` or `bottom`
                position: ""right"", // `left`, `center` or `right`
                stopOnFocus: true, // Prevents dismissing of toast on hover
                style:
                {
                    background: ""linear-gradient(to right, #00b09b, #96c93d)"",
                },
                onClick: function () { } // Callback after click
            }).showToast();
        </script>
");
#nullable restore
#line 27 "D:\Sources\Tizhoshan\Tizhoshan\Tizhoshan.Web\Views\Shared\_SiteMessages.cshtml"
    }

    if (TempData["error"] != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        <script>
            let message = $(""#showMessage2"").val();
            Toastify({
                text: message,
                duration: 3000,
                newWindow: true,
                close: false,
                gravity: ""top"", // `top` or `bottom`
                position: ""right"", // `left`, `center` or `right`
                stopOnFocus: true, // Prevents dismissing of toast on hover
                style:
                {
                    background: ""linear-gradient(to right, #ff0000, #ff4141)"",
                },
                onClick: function () { } // Callback after click
            }).showToast();
        </script>
");
#nullable restore
#line 48 "D:\Sources\Tizhoshan\Tizhoshan\Tizhoshan.Web\Views\Shared\_SiteMessages.cshtml"
    }

    if (TempData["info"] != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        <script>
            let message = $(""#showMessage3"").val();
            Toastify({
                text: message,
                duration: 3000,
                newWindow: true,
                close: false,
                gravity: ""top"", // `top` or `bottom`
                position: ""right"", // `left`, `center` or `right`
                stopOnFocus: true, // Prevents dismissing of toast on hover
                style:
                {
                    background: ""linear-gradient(to right, #0005ff, #393dff)"",
                },
                onClick: function () { } // Callback after click
            }).showToast();
        </script>
");
#nullable restore
#line 69 "D:\Sources\Tizhoshan\Tizhoshan\Tizhoshan.Web\Views\Shared\_SiteMessages.cshtml"
    }

    if (TempData["warning"] != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        <script>
            let message = $(""#showMessage4"").val();
            Toastify({
                text: message,
                duration: 3000,
                newWindow: true,
                close: false,
                gravity: ""top"", // `top` or `bottom`
                position: ""right"", // `left`, `center` or `right`
                stopOnFocus: true, // Prevents dismissing of toast on hover
                style:
                {
                    background: ""linear-gradient(to right, #fffc00, #fffd39)"",
                },
                onClick: function () { } // Callback after click
            }).showToast();
        </script>
");
#nullable restore
#line 90 "D:\Sources\Tizhoshan\Tizhoshan\Tizhoshan.Web\Views\Shared\_SiteMessages.cshtml"
    }
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\n");
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