using Domain.Entities;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.ChangeLogService
{
    public class ChangeLogService : IChangeLogService
    {
        private readonly ApplicationDbContext context;

        public ChangeLogService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<ChangeLog> GetChangeLogsByWhereClause(DateTime begindate, DateTime endDate, string entityName)
        {
            if (!string.IsNullOrEmpty(entityName))
            {
                return this.context.ChangeLogs.Where(x => x.ModifiedDate > begindate && (endDate != DateTime.MinValue ? x.ModifiedDate < endDate : x.ModifiedDate > endDate) && x.EntityName.Contains(entityName));
            }
            else
            {
                return this.context.ChangeLogs.Where(x => x.ModifiedDate > begindate && (endDate != DateTime.MinValue ? x.ModifiedDate < endDate : x.ModifiedDate > endDate));
            }            
        }

        public IEnumerable<ChangeLog> GetChangeLogs()
        {
            return this.context.ChangeLogs.AsEnumerable<ChangeLog>();
        }
    }
}
