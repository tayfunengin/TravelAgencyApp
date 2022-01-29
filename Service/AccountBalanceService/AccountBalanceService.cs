using Domain.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Service.AccountBalanceService
{
    public class AccountBalanceService : IAccountBalanceService
    {
        private readonly IRepository<AccountBalance> repository;

        public AccountBalanceService(IRepository<AccountBalance> repository)
        {
            this.repository = repository;
        }

        public void CreateAccountBalance(Sales sales)
        {

            List<AccountBalance> accountBalances = new List<AccountBalance>();

            AccountBalance accountBalanceForSales = new AccountBalance()
            {
                SellerId = sales.Id,
                AccountName = sales.SalesCode,
                CurrentDept = sales.SalesAmount,
                Sales = sales,
                Date = sales.SalesDate.Date
            };
            accountBalances.Add(accountBalanceForSales);

            AccountBalance accountBalanceForHotel = new AccountBalance()
            {
                SellerId = sales.Package.HotelPrice.HotelId,
                AccountName = sales.Package.HotelPrice.Hotel.HotelCode,
                CurrentCredit = sales.Package.HotelPrice.PurchasePrice * sales.Package.NightCount * sales.CurrencyRate,
                Sales = sales,
                Date = sales.SalesDate.Date
            };
            accountBalances.Add(accountBalanceForHotel);
            
     
            foreach (var flight in sales.Package.FlightPrices)
            {
                if (accountBalances.Last().AccountName == flight.FlightCompany.FlightCompanyCode)
                {
                    accountBalances.Last().CurrentCredit += flight.PurchasePrice * sales.Package.GuestCount * sales.CurrencyRate;
                }
                else
                {
                    AccountBalance accountBalanceForFlight = new AccountBalance()
                    {
                        SellerId = flight.FlightCompanyId,
                        AccountName = flight.FlightCompany.FlightCompanyCode,
                        CurrentCredit = flight.PurchasePrice * sales.Package.GuestCount * sales.CurrencyRate,
                        Sales = sales,
                        Date = sales.SalesDate.Date
                    };
                    accountBalances.Add(accountBalanceForFlight);
                }            
            }

            foreach (var transportation in sales.Package.TransportationPrices)
            {
                if (accountBalances.Last().AccountName == transportation.TransportationCompany.TransportaionCompanyCode)
                {
                    accountBalances.Last().CurrentCredit += transportation.PurchasePrice * sales.Package.GuestCount * sales.CurrencyRate;
                }
                else
                {
                    AccountBalance accountBalanceForTransportation = new AccountBalance()
                    {
                        SellerId = transportation.TransportationCompanyId,
                        AccountName = transportation.TransportationCompany.TransportaionCompanyCode,
                        CurrentCredit = transportation.PurchasePrice * sales.Package.GuestCount * sales.CurrencyRate,
                        Sales = sales,
                        Date = sales.SalesDate.Date
                    };
                    accountBalances.Add(accountBalanceForTransportation);
                }
              
            }

            foreach (var tour in sales.Package.TourPrices)
            {
                if (accountBalances.Last().AccountName == tour.TourOperator.TourOperatorCode)
                {
                    accountBalances.Last().CurrentCredit += tour.PurchasePrice * sales.Package.GuestCount * sales.CurrencyRate;
                }
                else
                {
                    AccountBalance accountBalanceForTour = new AccountBalance()
                    {
                        SellerId = tour.TourOperatorId,
                        AccountName = tour.TourOperator.TourOperatorCode,
                        CurrentCredit = tour.PurchasePrice * sales.Package.GuestCount * sales.CurrencyRate,
                        Sales = sales,
                        Date = sales.SalesDate.Date
                    };
                    accountBalances.Add(accountBalanceForTour);
                }             
            }

            if (sales.RentACarPriceId !=null)
            {
                AccountBalance accountBalanceForRentalCar = new AccountBalance()
                {
                    SellerId = sales.RentACarPrice.RentACarCompanyId,
                    AccountName = sales.RentACarPrice.RentACarCompany.RentACarCompanyCode,
                    CurrentCredit = sales.RentACarPrice.PurchasePrice * sales.RentACarDayCount.Value * sales.CurrencyRate,
                    Sales = sales,
                    Date = sales.SalesDate.Date
                };
                accountBalances.Add(accountBalanceForRentalCar);
            }

            if (sales.TourGuidePriceId != null)
            {
                AccountBalance accountBalanceTourGuide = new AccountBalance()
                {
                    SellerId = sales.TourGuidePrice.TourGuideId,
                    AccountName = sales.TourGuidePrice.TourGuide.TourGuideCode,
                    CurrentCredit = sales.TourGuidePrice.PurchasePrice * sales.CurrencyRate,
                    Sales = sales,
                    Date = sales.SalesDate.Date
                };
                accountBalances.Add(accountBalanceTourGuide);
            }

            using(var transactionScope = new TransactionScope())
            {
                foreach (var accountBalance in accountBalances)
                {
                    this.repository.Insert(accountBalance);
                }
                transactionScope.Complete();
            }
        }

        public AccountBalance GetById(int id)
        {
            return this.repository.GetById(id);
        }

        public List<AccountBalance> GetBySalesId(int salesId)
        {
            return this.repository.GetDefault(x => x.SalesId == salesId).ToList();
        }

        public List<AccountBalance> GetByWhereClause(string accountCode, DateTime beginDate, DateTime endDate)
        {
            List<AccountBalance> accountBalances = this.repository.GetDefault(x => x.AccountName == accountCode && (x.Date >= beginDate && x.Date <= endDate )).ToList();
            return accountBalances;
        }
    }
}
