using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class TourOperatorPriceMap
    {
        public TourOperatorPriceMap(EntityTypeBuilder<TourOperatorPrice> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.PurchasePrice).HasPrecision(18, 2).IsRequired();
            entityTypeBuilder.Property(x => x.SalePrice).HasPrecision(18, 2).IsRequired();
            entityTypeBuilder.Property(x => x.Period).IsRequired();

            entityTypeBuilder.HasOne(x => x.TourOperator).WithMany(x => x.Prices).HasForeignKey(x => x.TourOperatorId);
            entityTypeBuilder.HasOne(x => x.Tour).WithMany(x => x.TourOperatorPrices).HasForeignKey(x => x.TourId).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);
        }
    }
}
