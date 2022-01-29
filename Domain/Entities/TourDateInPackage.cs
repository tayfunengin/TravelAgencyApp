using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TourDateInPackage : BaseEntity
    {
        public int PackageId { get; set; }

        public virtual Package Package { get; set; }

        public virtual TourOperatorPrice TourPrice { get; set; }
        public int TourPriceId { get; set; }

        public DateTime TourDate { get; set; }
    }
}
