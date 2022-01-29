using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class FlightCompanyAndPlaneMap
    {
        public FlightCompanyAndPlaneMap(EntityTypeBuilder<FlightCompanyAndPlane> entityTypeBuilder)
        {
            entityTypeBuilder.Ignore(x => x.Id);
            entityTypeBuilder.HasKey(x => new { x.PlaneId, x.FlightCompanyId });
        }
    }
}
