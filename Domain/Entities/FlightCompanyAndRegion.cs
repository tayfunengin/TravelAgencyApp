using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class FlightCompanyAndRegion : BaseEntity
    {
        public int FlightCompanyId { get; set; }
        public int RegionId { get; set; }
        public FlightCompany FlightCompany { get; set; }
        public Region Region { get; set; }
    }
}
