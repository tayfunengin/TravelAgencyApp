using Domain.Entities;
using Domain.Enums;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.RentACarPriceService
{
    public class RentACarPriceService : IRentACarPriceService
    {
        private readonly IRepository<RentACarPrice> repository;

        public RentACarPriceService(IRepository<RentACarPrice> repository)
        {
            this.repository = repository;
        }
        public bool CheckIfThereIsPriceForCombination(RentACarPrice price)
        {
            List<RentACarPrice> prices = this.repository.GetDefault(x => x.RentACarCompanyId == price.RentACarCompanyId && x.Status == Convert.ToBoolean(Status.Active)).ToList();
            foreach (var item in prices)
            {
                if (item.CarId == price.CarId && item.Period == price.Period && item.Year == price.Year)
                    return true;
            }
            return false;
        }

        public string CreateRentACarPrice(RentACarPrice rentACarPrice, int companyId)
        {
            rentACarPrice.RentACarCompanyId = companyId;
            if (this.CheckIfThereIsPriceForCombination(rentACarPrice))
            {
                return NotificationType.warning + ":" + "There is already a defined price for the combination.";
            }
            return this.repository.Insert(rentACarPrice);
        }

        public string DeleteRentACarPrice(int id)
        {
            return this.repository.Delete(id);
        }

        public IEnumerable<RentACarPrice> GetActiveRentACarPrices()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active));
        }

        public RentACarPrice GetRentACarPriceById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<RentACarPrice> GetRentACarPrices()
        {
            return this.repository.GetAll();
        }

        public string UpdateRentACarPrice(RentACarPrice rentACarPrice)
        {
            return this.repository.Update(rentACarPrice);
        }
    }
}
