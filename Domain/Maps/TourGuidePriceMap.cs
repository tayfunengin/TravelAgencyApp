using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class TourGuidePriceMap
    {
        public TourGuidePriceMap(EntityTypeBuilder<TourGuidePrice> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.PurchasePrice).HasPrecision(18, 2).IsRequired();
            entityTypeBuilder.Property(x => x.SalePrice).HasPrecision(18, 2).IsRequired();
            entityTypeBuilder.Property(x => x.Period).IsRequired();

            entityTypeBuilder.HasOne(x => x.TourGuide).WithMany(x => x.Prices).HasForeignKey(x => x.TourGuideId);
        }
    }
}
