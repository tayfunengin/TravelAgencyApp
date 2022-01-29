using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Sales : BaseEntity
    {
        public string SalesCode { get; set; }

        public DateTime SalesDate { get; set; }

        public decimal CurrencyRate { get; set; }
        public decimal SalesAmount { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int PackageId { get; set; }
        public virtual Package Package { get; set; }

        public DateTime? RentACarStartDate { get; set; }
        public DateTime? RentACarEndDate { get; set; }
        public byte? RentACarDayCount { get; set; }
        public int? RentACarPriceId { get; set; }
        public virtual RentACarPrice RentACarPrice { get; set; }


        public DateTime? TourGuideRentDay { get; set; }
        public int? TourGuidePriceId { get; set; }
        public virtual TourGuidePrice TourGuidePrice { get; set; }

        public virtual List<AccountBalance> AccountBalances { get; set; }

    }
}
