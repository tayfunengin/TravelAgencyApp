#pragma checksum "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8fd82c80e07ce3ed81ce63c76859623d12e99a9b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Dashboard_Views_Sales_AddRentACar), @"mvc.1.0.view", @"/Areas/Dashboard/Views/Sales/AddRentACar.cshtml")]
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
#line 1 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\_ViewImports.cshtml"
using Domain.Entities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\_ViewImports.cshtml"
using Repository.Authentication;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\_ViewImports.cshtml"
using Repository.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\_ViewImports.cshtml"
using UI.Tools;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\_ViewImports.cshtml"
using X.PagedList;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\_ViewImports.cshtml"
using X.PagedList.Mvc.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\_ViewImports.cshtml"
using X.PagedList.Web.Common;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8fd82c80e07ce3ed81ce63c76859623d12e99a9b", @"/Areas/Dashboard/Views/Sales/AddRentACar.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2fd4946acc6b599e605cd38b45170112243039da", @"/Areas/Dashboard/Views/_ViewImports.cshtml")]
    public class Areas_Dashboard_Views_Sales_AddRentACar : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<RentACarPrice>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("submit"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-toggle", new global::Microsoft.AspNetCore.Html.HtmlString("tooltip"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("title", new global::Microsoft.AspNetCore.Html.HtmlString("Choose"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-placement", new global::Microsoft.AspNetCore.Html.HtmlString("bottom"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn add"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("choose"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ChooseRentACar", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
  
    Layout = "~/Areas/Dashboard/Views/Shared/_DashboardLayout.cshtml";
    DateTime startDate = Convert.ToDateTime(TempData["startDate"]);
    var startYear = startDate.Year;
    string startMonth = startDate.Month.ToString();
    if (startMonth.ToCharArray().Length < 2)
        startMonth = "0" + startMonth;

    string startDay = startDate.Day.ToString();

    if (startDay.ToCharArray().Length < 2)
    {
        startDay = "0" + startDay;
    }

    DateTime endDate = Convert.ToDateTime(TempData["endDate"]);
    var endYear = endDate.Year;
    string endMonth = endDate.Month.ToString();
    if (endMonth.ToCharArray().Length < 2)
    {
        endMonth = "0" + endMonth;
    }

    string endDay = endDate.Day.ToString();
    if (endDay.ToCharArray().Length < 2)
    {
        endDay = "0" + endDay;
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"card\">\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8fd82c80e07ce3ed81ce63c76859623d12e99a9b8465", async() => {
                WriteLiteral(@"
        <div class=""card-header col-lg-12"">
            <div class=""row"">
                <div class=""col-md-9"">
                    <h4>Rent A Car Price List</h4>
                    <div class=""row"">
                        <div class=""col-lg-6"">
                            <label>Begin Date</label>
                            <input type=""date"" id=""beginDate"" name=""begindate"" class=""form-control""");
                BeginWriteAttribute("min", " min=\"", 1362, "\"", 1411, 5);
#nullable restore
#line 41 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1368, startYear.ToString(), 1368, 21, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1389, "-", 1389, 1, true);
#nullable restore
#line 41 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1390, startMonth, 1390, 11, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1401, "-", 1401, 1, true);
#nullable restore
#line 41 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1402, startDay, 1402, 9, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("max", " max=\"", 1412, "\"", 1455, 5);
#nullable restore
#line 41 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1418, endYear.ToString(), 1418, 19, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1437, "-", 1437, 1, true);
#nullable restore
#line 41 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1438, endMonth, 1438, 9, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1447, "-", 1447, 1, true);
#nullable restore
#line 41 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1448, endDay, 1448, 7, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                        </div>\r\n                        <div class=\"col-lg-6\">\r\n\r\n                            <label>End Date</label>\r\n                            <input type=\"date\" id=\"endDate\" name=\"endDate\" class=\"form-control\"");
                BeginWriteAttribute("min", " min=\"", 1691, "\"", 1729, 5);
#nullable restore
#line 46 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1697, startYear, 1697, 10, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1707, "-", 1707, 1, true);
#nullable restore
#line 46 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1708, startMonth, 1708, 11, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1719, "-", 1719, 1, true);
#nullable restore
#line 46 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1720, startDay, 1720, 9, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("max", " max=\"", 1730, "\"", 1762, 5);
#nullable restore
#line 46 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1736, endYear, 1736, 8, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1744, "-", 1744, 1, true);
#nullable restore
#line 46 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1745, endMonth, 1745, 9, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1754, "-", 1754, 1, true);
#nullable restore
#line 46 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1755, endDay, 1755, 7, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n                <div class=\"col-md-3 text-right\">\r\n                    <a");
                BeginWriteAttribute("href", " href=\"", 1925, "\"", 1952, 1);
#nullable restore
#line 51 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
WriteAttributeValue("", 1932, TempData["referer"], 1932, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" data-toggle=""tooltip"" title=""Go Back"" data-placement=""bottom"">
                        <i class=""fas fa-arrow-left fa-2x text-muted""></i>
                    </a>
                </div>
            </div>
        </div>
        <div class=""card-body col-lg-12"">
            <table id=""priceTable"" class=""table table-hover table-responsive-md"">
                <thead>
                    <tr>
                        <th>Company Name</th>
                        <th>Car Brand</th>
                        <th>Car Model</th>
                        <th>Year</th>
                        <th>Period</th>
                        <th>Price Per Day(Eur)</th>
                        <th> </th>
                    </tr>
                </thead>
                <tbody>
");
#nullable restore
#line 71 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
                     foreach (var price in Model)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <tr>\r\n                            <td>");
#nullable restore
#line 74 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
                           Write(price.RentACarCompany.RentACarCompanyName);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 75 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
                           Write(price.Car.Brand);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 76 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
                           Write(price.Car.Model);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 77 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
                           Write(price.Year);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 78 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
                           Write(price.Period);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 79 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
                           Write(price.SalePrice);

#line default
#line hidden
#nullable disable
                WriteLiteral(" &euro;</td>\r\n                            <td>\r\n                                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("button", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8fd82c80e07ce3ed81ce63c76859623d12e99a9b17420", async() => {
                    WriteLiteral("\r\n                                    <i class=\"far fa-check-circle text-success\"></i>\r\n                                ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues == null)
                {
                    throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-priceId", "Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper", "RouteValues"));
                }
                BeginWriteTagHelperAttribute();
#nullable restore
#line 81 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
                                                             WriteLiteral(price.Id);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues["priceId"] = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-priceId", __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues["priceId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                            </td>\r\n                        </tr>\r\n");
#nullable restore
#line 86 "C:\Users\Tayfun\Documents\.NET EĞİTİM\Resital\TravelAgencyApplication\UI\Areas\Dashboard\Views\Sales\AddRentACar.cshtml"
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("                </tbody>\r\n            </table>\r\n        </div>\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n\r\n");
            DefineSection("scripts", async() => {
                WriteLiteral(@"
    <script>

        $(document).ready(function () {

            $('#priceTable').DataTable();

            $("".add"").click(function () {
                sessionStorage.setItem(""parentid"", ""SalesAgents"");
                sessionStorage.setItem(""id"", ""SalesAgent"");
            });

            $(""#choose"").click(function (e) {

                var begin = $(""#beginDate"").val();
                var end = $(""#endDate"").val();

                if (begin == """" || begin == undefined || end == """" || end == undefined) {
                    alert(""Please choose begin/end date!"")
                    e.preventDefault();
                }
                else {
                    if (begin > end) {
                        alert(""End date can not be earlier than begin date!"")
                        e.preventDefault();
                    }
                }

            });

        });
    </script>

");
            }
            );
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<RentACarPrice>> Html { get; private set; }
    }
}
#pragma warning restore 1591
