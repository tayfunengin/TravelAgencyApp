using Domain.Entities;
using Domain.Enums;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.TourOperatorPriceService
{
    public class TourOperatorPriceService : ITourOperatorPriceService
    {
        private readonly IRepository<TourOperatorPrice> repository;

        public TourOperatorPriceService(IRepository<TourOperatorPrice> repository)
        {
            this.repository = repository;
        }

        public bool CheckIfThereIsPriceForCombination(TourOperatorPrice price)
        {
            List<TourOperatorPrice> prices = this.repository.GetDefault(x => x.TourOperatorId == price.TourOperatorId && x.Status == Convert.ToBoolean(Status.Active)).ToList();
            foreach (var item in prices)
            {
                if (item.TourId == price.TourId && item.Period == price.Period && item.Year == price.Year)
                    return true;
            }
            return false;
        }

        public string CreateTourOperatorPrice(TourOperatorPrice tourOperatorPrice, int operatorId)
        {
            tourOperatorPrice.TourOperatorId = operatorId;          
            if (this.CheckIfThereIsPriceForCombination(tourOperatorPrice))
            {
                return NotificationType.warning + ":" + "There is already a defined price for the combination.";
            }
            return this.repository.Insert(tourOperatorPrice);
        }

        public string DeleteTourOperatorPrice(int id)
        {
            return this.repository.Delete(id);
        }

        public IEnumerable<TourOperatorPrice> GetActiveTourOperatorPrices()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active));
        } 

        public TourOperatorPrice GetTourOperatorPriceById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<TourOperatorPrice> GetTourOperatorPrices()
        {
            return this.repository.GetAll();
        }

        public string UpdateTourOperatorPrice(TourOperatorPrice tourOperatorPrice)
        {
            return this.repository.Update(tourOperatorPrice);
        }
    }
}
