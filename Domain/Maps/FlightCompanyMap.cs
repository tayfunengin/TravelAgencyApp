using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class FlightCompanyMap
    {
        public FlightCompanyMap(EntityTypeBuilder<FlightCompany> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.FlightCompanyCode).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.FlightCompanyName).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Address).IsRequired().HasMaxLength(250);          
        }
    }
}
