using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.UserLogService
{
    public interface IUserLogService
    {     
        IEnumerable<UserLog> GetUserLogs();

        IEnumerable<UserLog> GetUserLogsByWhereClause(DateTime begindate, DateTime endDate,string userName);
    }
}
