using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.ChangeLogService
{
    public interface IChangeLogService
    {
        IEnumerable<ChangeLog> GetChangeLogs();

        IEnumerable<ChangeLog> GetChangeLogsByWhereClause(DateTime begindate, DateTime endDate, string entityName);
    }
}
