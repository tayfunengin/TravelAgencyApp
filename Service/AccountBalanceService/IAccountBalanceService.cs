using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.AccountBalanceService
{
    public interface IAccountBalanceService
    {
        void CreateAccountBalance(Sales sales);
        AccountBalance GetById(int id);

        List<AccountBalance> GetBySalesId(int salesId);

        List<AccountBalance> GetByWhereClause(string accountCode, DateTime beginDate, DateTime endDate);
    }
}
