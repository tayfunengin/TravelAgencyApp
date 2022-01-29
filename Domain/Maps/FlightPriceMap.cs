using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class FlightPriceMap
    {
        public FlightPriceMap(EntityTypeBuilder<FlightPrice> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.PurchasePrice).HasPrecision(18, 2).IsRequired();
            entityTypeBuilder.Property(x => x.SalePrice).HasPrecision(18, 2).IsRequired();
            entityTypeBuilder.Property(x => x.Period).IsRequired();

            entityTypeBuilder.HasOne(x => x.FlightCompany).WithMany(x => x.Prices).HasForeignKey(x => x.FlightCompanyId);
        }
    }
}
