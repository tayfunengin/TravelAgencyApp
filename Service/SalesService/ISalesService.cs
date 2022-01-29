using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.SalesService
{
    public interface ISalesService
    {      
        string GenerateSalesCode();
        string CreateSales(Package package, Customer customer);

        Sales GetById(int id);
        IEnumerable<Sales> GetAllSales();

        IEnumerable<Sales> GetByRegionAndSalesDate(DateTime salesDate, int regionId);

        IEnumerable<Sales> GetByWhereClause(DateTime begindate, DateTime endDate, string salesCode);
    }
}
