using Domain.Entities;
using Repository;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.UserLogService
{
    public class UserLogService : IUserLogService
    {
        private readonly ApplicationDbContext context;

        public UserLogService(ApplicationDbContext context) 
        {
            this.context = context;
        }
        public IEnumerable<UserLog> GetUserLogs()
        {
            return context.UserLogs.AsEnumerable<UserLog>();
        }

        public IEnumerable<UserLog> GetUserLogsByWhereClause(DateTime begindate, DateTime endDate, string userName)
        {
            
            List<UserLog> userLogs = new List<UserLog>();

            if (!string.IsNullOrEmpty(userName))
            {
                userLogs = this.context.UserLogs.Where(x => x.LogDate > begindate && (endDate != DateTime.MinValue ? x.LogDate < endDate : x.LogDate > endDate) && x.UserName == userName).ToList();
            }
            else
            {
                userLogs = this.context.UserLogs.Where(x => x.LogDate > begindate && (endDate != DateTime.MinValue ? x.LogDate < endDate : x.LogDate > endDate)).ToList();
            }     
            
            return userLogs;
        }
    }
}
