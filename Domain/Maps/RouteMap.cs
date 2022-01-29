using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class RouteMap
    {
        public RouteMap(EntityTypeBuilder<Route> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.RouteCode).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Description).IsRequired().HasMaxLength(100);           
        }

    }
}
