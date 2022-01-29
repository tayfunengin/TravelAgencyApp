using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.FlightPriceService
{
    public interface IFlightPriceService
    {
        IEnumerable<FlightPrice> GetFlightPrices();

        IEnumerable<FlightPrice> GetActiveFlightPrices();

        FlightPrice GetFlightPriceById(int id);

        string CreateFlightPrice(FlightPrice flightPrice, int companyId);

        string UpdateFlightPrice(FlightPrice flightPrice);

        string DeleteFlightPrice(int id);

        bool CheckIfThereIsPriceForCombination(FlightPrice price);

       
    }
}
