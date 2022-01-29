using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class TransportationPriceMap
    {
        public TransportationPriceMap(EntityTypeBuilder<TransportationPrice> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.PurchasePrice).HasPrecision(18, 2).IsRequired();
            entityTypeBuilder.Property(x => x.SalePrice).HasPrecision(18, 2).IsRequired();
            entityTypeBuilder.Property(x => x.Period).IsRequired();

            entityTypeBuilder.HasOne(x => x.TransportationCompany).WithMany(x => x.Prices).HasForeignKey(x => x.TransportationCompanyId).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);
        }
    }
}
