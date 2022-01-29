using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TransportationCompanyAndRoute : BaseEntity
    {
        public int TransportationCompanyId { get; set; }
        public int RouteId { get; set; }
        public virtual TransportationCompany TransportationCompany { get; set; }
        public virtual Route Route { get; set; }
    }
}
