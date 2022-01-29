using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.TourGuidePriceService
{
    public interface ITourGuidePriceService
    {
        IEnumerable<TourGuidePrice> GetTourGuidePrices();

        IEnumerable<TourGuidePrice> GetActiveTourGuidePrices();

        TourGuidePrice GetTourGuidePriceById(int id);

        string CreateTourGuidePrice(TourGuidePrice tourGuidePrice/*, int companyId*/);

        string UpdateTourGuidePrice(TourGuidePrice tourGuidePrice);

        string DeleteTourGuidePrice(int id);

        bool CheckIfThereIsPriceForCombination(TourGuidePrice price);

    }

}
