using Domain.Entities;
using Domain.Enums;
using Repository;
using Service.HotelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Service.HotelPriceService
{
    public class HotelPriceService : IHotelPriceService
    {
        private readonly IRepository<HotelPrice> repository;
        private readonly IHotelService hotelService;

        public HotelPriceService(IRepository<HotelPrice> repository, IHotelService hotelService)
        {
            this.repository = repository;
            this.hotelService = hotelService;
        }
        public bool CheckIfThereIsPriceForCombination(HotelPrice hotelPrice)
        {           
            List<HotelPrice> hotelPrices = this.repository.GetDefault(x => x.HotelId == hotelPrice.HotelId && x.Status == Convert.ToBoolean(Status.Active)).ToList();
            foreach (var item in hotelPrices)
            {
                if (item.AccomodationTypeId == hotelPrice.AccomodationTypeId && item.RoomLocationId == hotelPrice.RoomLocationId && item.RoomTypeId == hotelPrice.RoomTypeId && item.Period == hotelPrice.Period && item.Year == hotelPrice.Year)
                    return true;
            }
            return false;
        }

        public string CreateHotelPrice(HotelPrice hotelPrice, int hotelId)
        {
            hotelPrice.HotelId = hotelId;
            if (this.CheckIfThereIsPriceForCombination(hotelPrice))
            {
                return NotificationType.warning + ":" + "There is already a defined price for the combination.";
            }
            return this.repository.Insert(hotelPrice);
        }

        public string DeleteHotelPrice(int id)
        {
            return this.repository.Delete(id);
        }

        public IEnumerable<HotelPrice> GetActiveHotelPrices()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active));
        }

        public HotelPrice GetHotelPriceById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<HotelPrice> GetHotelPrices()
        {
            return this.repository.GetAll();
        }

        public string UpdateHotelPrice(HotelPrice hotelPrice)
        {                    
            return this.repository.Update(hotelPrice);
        }
    }
}
