using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Maps
{
    public class FlightCompanyAndRegionMap
    {
        public FlightCompanyAndRegionMap(EntityTypeBuilder<FlightCompanyAndRegion> entityTypeBuilder)
        {
            entityTypeBuilder.Ignore(x => x.Id);
            entityTypeBuilder.HasKey(x => new { x.RegionId, x.FlightCompanyId });
        }
    }
}
