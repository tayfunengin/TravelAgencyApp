using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class TransportationCompanyAndRouteMap
    {
        public TransportationCompanyAndRouteMap(EntityTypeBuilder<TransportationCompanyAndRoute> entityTypeBuilder)
        {
            entityTypeBuilder.Ignore(x => x.Id);
            entityTypeBuilder.HasKey(x => new { x.TransportationCompanyId, x.RouteId });            
        }
    }
}
