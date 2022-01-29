using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class FlightCompanyAndPlane : BaseEntity
    {    
        public int FlightCompanyId { get; set; }
        public int PlaneId { get; set; }
        public FlightCompany FlightCompany { get; set; }
        public Plane Plane { get; set; }
    }
}
