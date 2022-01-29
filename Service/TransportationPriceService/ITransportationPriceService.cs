using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.TransportationPriceService
{
    public interface ITransportationPriceService
    {
        IEnumerable<TransportationPrice> GetTransportationPrices();

        IEnumerable<TransportationPrice> GetActiveTransportationPrices();

        TransportationPrice GetTransportationPriceById(int id);

        string CreateTransportationPrice(TransportationPrice transportationPrice, int companyId);

        string UpdateTransportationPrice(TransportationPrice transportationPrice);

        string DeleteTransportationPrice(int id);

        bool CheckIfThereIsPriceForCombination(TransportationPrice price);
    }
}
