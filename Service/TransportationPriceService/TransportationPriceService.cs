using Domain.Entities;
using Domain.Enums;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.TransportationPriceService
{
    public class TransportationPriceService : ITransportationPriceService
    {
        private readonly IRepository<TransportationPrice> repository;

        public TransportationPriceService(IRepository<TransportationPrice> repository)
        {
            this.repository = repository;
        }

        public bool CheckIfThereIsPriceForCombination(TransportationPrice price)
        {
            List<TransportationPrice> prices = this.repository.GetDefault(x => x.TransportationCompanyId == price.TransportationCompanyId && x.Status == Convert.ToBoolean(Status.Active)).ToList();
            foreach (var item in prices)
            {
                if (item.RouteId == price.RouteId && item.Period == price.Period && item.Year == price.Year)
                    return true;
            }
            return false;
        }

        public string CreateTransportationPrice(TransportationPrice transportationPrice, int companyId)
        {
            transportationPrice.TransportationCompanyId = companyId;
            if (this.CheckIfThereIsPriceForCombination(transportationPrice))
            {
                return NotificationType.warning + ":" + "There is already a defined price for the combination.";
            }
            return this.repository.Insert(transportationPrice);
        }

        public string DeleteTransportationPrice(int id)
        {
            return this.repository.Delete(id);
        }

        public IEnumerable<TransportationPrice> GetActiveTransportationPrices()
        {
            return this.repository.GetDefault(x => x.Status == Convert.ToBoolean(Status.Active));
        }

        public TransportationPrice GetTransportationPriceById(int id)
        {
            return this.repository.GetById(id);
        }

        public IEnumerable<TransportationPrice> GetTransportationPrices()
        {
            return this.repository.GetAll();
        }

        public string UpdateTransportationPrice(TransportationPrice transportationPrice)
        {
            return this.repository.Update(transportationPrice);
        }
    }
}
