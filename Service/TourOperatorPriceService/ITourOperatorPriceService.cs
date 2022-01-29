using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.TourOperatorPriceService
{
    public interface ITourOperatorPriceService
    {
        IEnumerable<TourOperatorPrice> GetTourOperatorPrices();

        IEnumerable<TourOperatorPrice> GetActiveTourOperatorPrices();

        TourOperatorPrice GetTourOperatorPriceById(int id);

        string CreateTourOperatorPrice(TourOperatorPrice tourOperatorPrice, int operatorId);

        string UpdateTourOperatorPrice(TourOperatorPrice tourOperatorPrice);

        string DeleteTourOperatorPrice(int id);

        bool CheckIfThereIsPriceForCombination(TourOperatorPrice price);

    }
}
