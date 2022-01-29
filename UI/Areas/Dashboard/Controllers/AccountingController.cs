using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.AccountBalanceService;
using Service.SalesService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Filters;
using UI.Tools;

namespace UI.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [TypeFilter(typeof(AcFilter))]
    [TypeFilter(typeof(AuthFilter))]
    [Authorize(Roles = "Admin,Accounting")]
    public class AccountingController : Controller
    {
        private readonly IAccountBalanceService accountBalanceService;
        private readonly ISalesService salesService;
        GoBackUrl goBackUrl;

        public AccountingController(IAccountBalanceService accountBalanceService, ISalesService salesService, IHttpContextAccessor httpContextAccessor)
        {
            this.accountBalanceService = accountBalanceService;
            this.salesService = salesService;
            goBackUrl = new GoBackUrl(httpContextAccessor);
        }
        public IActionResult SalesReports()
        {    
            return View(salesService.GetAllSales());
        }

        [HttpPost]
        public IActionResult SalesReports(string salesCode, DateTime beginDate, DateTime endDate )
        {
            return View(salesService.GetByWhereClause(beginDate,endDate,salesCode));
        }

        public IActionResult ReportDetails(int salesId)
        {
            Sales sales = salesService.GetById(salesId);
            List<AccountBalance> accountBalances = accountBalanceService.GetBySalesId(salesId);
            if (accountBalances.Count > 0)
            {
                ViewBag.sales = sales;
                TempData["referer"] = goBackUrl.GetBackUrl();
                TempData.Keep("referer");
                return View(accountBalances);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "There is no current balance for given sales code. ");
                return RedirectToAction("SalesReports");
            }
        }

        public IActionResult AccountReports()
        {
            List<AccountBalance> accountBalances = new List<AccountBalance>();  
            return View(accountBalances);
        }

        [HttpPost]
        public IActionResult AccountReports(string accountCode, DateTime beginDate, DateTime endDate)
        {
            ViewBag.start = beginDate.ToShortDateString();
            ViewBag.end = endDate.ToShortDateString();
            return View(accountBalanceService.GetByWhereClause(accountCode,beginDate,endDate));
        }
        
    }
}
