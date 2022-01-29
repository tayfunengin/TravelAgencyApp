using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Filters
{
    public class AcFilter : Attribute, IActionFilter
    {

        private readonly ApplicationDbContext applicationDbContext;
        public AcFilter(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            UserLog log = new UserLog();

            log.ActionName = context.ActionDescriptor.DisplayName.Split('.').Last();
            log.ControllerName = context.Controller.ToString().Split('.').Last();
            log.LogDate = DateTime.Now;
            foreach (var key in context.HttpContext.Session.Keys)
            {
                if (key == "login")
                {                    
                    string[] usernameAndRole = context.HttpContext.Session.GetString("login").Split(',');
                    log.UserName = usernameAndRole[0];
                    log.Role = usernameAndRole[1];
                    log.Description = "Visited by " + log.UserName;
                    applicationDbContext.UserLogs.Add(log);
                    applicationDbContext.SaveChanges();
                    return;
                }
            }
            log.UserName = "bilinmeyen kullanıcı";
            log.Role = "bilinmeyen rol";
            log.Description = "Visited by " + log.UserName ;
            applicationDbContext.UserLogs.Add(log);
            applicationDbContext.SaveChanges();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
