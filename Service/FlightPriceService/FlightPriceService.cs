using Domain.Entities;
using Domain.Enums;
using Repository;
using Service.PackageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.FlightPriceService
{
    public class FlightPriceService : IFlightPriceService
    {
        private readonly IRepository<FlightPrice> repository;       

        public FlightPriceService(IRepository<FlightPrice> repository)
        {
            this.repository = repository;
        }
        public bool CheckIfThereIsPriceForCombination(FlightPrice price)
        {
            List<FlightPrice> prices = this.repository.GetDefault(x => x.FlightCompanyId == price.FlightCompanyId && x.Status == Convert.ToBoolean(Status.Active)).ToList();
            foreach (var item in prices)
            {
                if (item.RouteId == price.RouteId && item.PlaneId == price.PlaneId && item.Period == price.Period && item.Year == price.Year)
                    return true;
            }
            return false;
        }

        public string CreateFlightPrice(FlightPrice flightPrice, int companyId)
        {
            flightPrice.FlightCompanyId = companyId;
            if (this.CheckIfThereIsPriceForCombination(flightPrice))
            {
                return NotificationType.warning + ":" + "There is already a defined price for the combination.";
            }
            return this.repository.Insert(flightPrice);
        }

        public string DeleteFlightPrice(int id)
        {
            return this.repository.Delete(id);
        }

        public IEnumerable<FlightPrice> GetActiveFlightPrices()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active));
        }

        public FlightPrice GetFlightPriceById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<FlightPrice> GetFlightPrices()
        {
            return this.repository.GetAll();
        }

        public string UpdateFlightPrice(FlightPrice flightPrice)
        {
            return this.repository.Update(flightPrice);
        }
    }
}
