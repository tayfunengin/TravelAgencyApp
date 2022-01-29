using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.HotelPriceService
{
    public interface IHotelPriceService
    {
        IEnumerable<HotelPrice> GetHotelPrices();

        IEnumerable<HotelPrice> GetActiveHotelPrices();

        HotelPrice GetHotelPriceById(int id);

        string CreateHotelPrice(HotelPrice hotelPrice, int hotelId);

        string UpdateHotelPrice(HotelPrice hotelPrice);

        string DeleteHotelPrice(int id);
        bool CheckIfThereIsPriceForCombination(HotelPrice hotelPrice);
    
    }
}
