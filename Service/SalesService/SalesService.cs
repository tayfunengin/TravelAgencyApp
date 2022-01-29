using Common;
using Domain.Entities;
using Repository;
using Service.AccountBalanceService;
using Service.CustomerService;
using Service.PackageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Service.SalesService
{
    public class SalesService : ISalesService
    {
        private readonly IRepository<Sales> repository;
        private readonly ICustomerService customerService;
        private readonly IRepository<SalesCodeCounter> salesCodeRepo;
        private readonly IPackageService packageService;
        private readonly IAccountBalanceService accountBalanceService;

        public SalesService(IRepository<Sales> repository, ICustomerService customerService, IRepository<SalesCodeCounter> salesCodeRepo, IPackageService packageService,IAccountBalanceService accountBalanceService)
        {
            this.repository = repository;
            this.customerService = customerService;
            this.salesCodeRepo = salesCodeRepo;
            this.packageService = packageService;
            this.accountBalanceService = accountBalanceService;
        }

        public string CreateSales(Package package, Customer customer)
        {
            using(var transactionScope = new TransactionScope())
            {
                string result = "";  
                Sales sales = new Sales();
                sales.SalesDate = DateTime.Now;
                decimal euroRate = GetCurrencyRate.GetByCurrency("EUR");
                sales.CurrencyRate = euroRate;
                sales.SalesAmount = package.Total * euroRate;            
                sales.Customer = customer;
                sales.Package = package;
                if (package.RentACarPriceId !=null)
                {
                    sales.RentACarPrice = package.RentACarPrice;
                    sales.RentACarStartDate = package.RentACarStartDate;
                    sales.RentACarEndDate = package.RentACarEndDate;
                    sales.RentACarDayCount = package.RentACarDayCount;
                }
                if (package.TourGuidePriceId !=null)
                {
                    sales.TourGuidePrice = package.TourGuidePrice;
                    sales.TourGuideRentDay = package.TourGuideRentDay;
                }
                sales.SalesCode = this.GenerateSalesCode();
                result = this.repository.Insert(sales);
                package.RentACarPriceId = null;
                package.RentACarStartDate = null;
                package.RentACarEndDate = null;
                package.TourGuidePriceId = null;
                package.TourGuideRentDay = null;
                if (package.Quota >0)
                {
                    package.Quota--;
                }
                this.accountBalanceService.CreateAccountBalance(sales);
                transactionScope.Complete();
                return result;
            }
      
        }

        public string GenerateSalesCode()
        {
           List<SalesCodeCounter> salesCodes = this.salesCodeRepo.GetAll().ToList();
            if (salesCodes.Count == 0)
            {
                SalesCodeCounter salesCode = new SalesCodeCounter()
                {
                    CodeChar = 'S',
                    CodeNumber = 5000
                };
                this.salesCodeRepo.Insert(salesCode);
                return salesCode.CodeChar + salesCode.CodeNumber.ToString();
            }
            else
            {                
                SalesCodeCounter salesCodeUpdated = salesCodeRepo.GetById(salesCodes[0].Id);
                salesCodeUpdated.CodeNumber++;
                salesCodeRepo.Update(salesCodeUpdated);
                return salesCodeUpdated.CodeChar + salesCodeUpdated.CodeNumber.ToString();
            }
        }

        public IEnumerable<Sales> GetAllSales()
        {
            return this.repository.GetAll();
        }

        public Sales GetById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<Sales> GetByRegionAndSalesDate(DateTime salesDate, int regionId)
        {
            List<Sales> sales = new List<Sales>();

            if (regionId != 0)
            {
                sales = this.repository.GetDefault(x => x.SalesDate > salesDate && x.Package.RegionId == regionId).ToList();
            }
            else 
            {
                sales = this.repository.GetDefault(x => x.SalesDate > salesDate ).ToList();
            }         
            return sales;
        }

        public IEnumerable<Sales> GetByWhereClause(DateTime begindate, DateTime endDate, string salesCode)
        {
            List<Sales> sales = new List<Sales>();

            if (!string.IsNullOrEmpty(salesCode))
            {
                sales = this.repository.GetAll().Where(x => x.SalesDate > begindate && (endDate != DateTime.MinValue ? x.SalesDate < endDate : x.SalesDate > endDate) && x.SalesCode == salesCode).ToList();
            }
            else
            {
                sales = this.repository.GetAll().Where(x => x.SalesDate > begindate && (endDate != DateTime.MinValue ? x.SalesDate < endDate : x.SalesDate > endDate)).ToList();
            }

            return sales;
        }
    }
}
