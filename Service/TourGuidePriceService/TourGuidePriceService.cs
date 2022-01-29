using Domain.Entities;
using Domain.Enums;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.TourGuidePriceService
{
    public class TourGuidePriceService : ITourGuidePriceService
    {
        private readonly IRepository<TourGuidePrice> repository;

        public TourGuidePriceService(IRepository<TourGuidePrice> repository)
        {
            this.repository = repository;
        }
        public bool CheckIfThereIsPriceForCombination(TourGuidePrice price)
        {
            List<TourGuidePrice> prices = this.repository.GetDefault(x => x.TourGuideId == price.TourGuideId && x.Status == Convert.ToBoolean(Status.Active)).ToList();
            foreach (var item in prices)
            {
                if (item.Period == price.Period && item.Year == price.Year)
                    return true;
            }
            return false;
        }

        public string CreateTourGuidePrice(TourGuidePrice tourGuidePrice)
        {          
            if (this.CheckIfThereIsPriceForCombination(tourGuidePrice))
            {
                return NotificationType.warning + ":" + "There is already a defined price for the combination.";
            }
            return this.repository.Insert(tourGuidePrice);
        }

        public string DeleteTourGuidePrice(int id)
        {
            return this.repository.Delete(id);
        }

        public IEnumerable<TourGuidePrice> GetActiveTourGuidePrices()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active));
        }

        public TourGuidePrice GetTourGuidePriceById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<TourGuidePrice> GetTourGuidePrices()
        {
            return this.repository.GetAll();
        }

        public string UpdateTourGuidePrice(TourGuidePrice tourGuidePrice)
        {
            return this.repository.Update(tourGuidePrice);
        }
    }
}
