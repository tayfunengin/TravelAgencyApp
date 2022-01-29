using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.UserLogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Mvc.Core;
using X.PagedList;
using Domain.Entities;
using UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Repository.Authentication;
using Service.ChangeLogService;

namespace UI.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [TypeFilter(typeof(AuthFilter))]
    [Authorize(Roles = "Admin")]
    public class LogController : Controller
    {
        private readonly IUserLogService userLogService;
        private readonly UserManager<AppUser> userManager;
        private readonly IChangeLogService changeLogService;

        public LogController(IUserLogService userLogService, UserManager<AppUser> userManager, IChangeLogService changeLogService)
        {
            this.userLogService = userLogService;
            this.userManager = userManager;
            this.changeLogService = changeLogService;
        }
        public ActionResult UserLogs()
        {
            TempData["userNames"] = new SelectList(userManager.Users.ToList());          
            return View(userLogService.GetUserLogs());
        }

        [HttpPost]
        public ActionResult UserLogs(DateTime beginDate, DateTime endDate, string userName)
        {
            TempData["userNames"] = new SelectList(userManager.Users.ToList());
            return View(userLogService.GetUserLogsByWhereClause(beginDate, endDate, userName));
        }

        public ActionResult ChangeLogs()
        {                        
            return View(changeLogService.GetChangeLogs());
        }

        [HttpPost]
        public ActionResult ChangeLogs(DateTime beginDate, DateTime endDate, string entityName)
        {           
            return View(changeLogService.GetChangeLogsByWhereClause(beginDate, endDate, entityName));
        }
   
    }
}
