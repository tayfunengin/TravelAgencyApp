using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.RentACarPriceService
{
    public interface IRentACarPriceService
    {
        IEnumerable<RentACarPrice> GetRentACarPrices();

        IEnumerable<RentACarPrice> GetActiveRentACarPrices();

        RentACarPrice GetRentACarPriceById(int id);

        string CreateRentACarPrice(RentACarPrice rentACarPrice, int companyId);

        string UpdateRentACarPrice(RentACarPrice rentACarPrice);

        string DeleteRentACarPrice(int id);

        bool CheckIfThereIsPriceForCombination(RentACarPrice price);
    }
}
