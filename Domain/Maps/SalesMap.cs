using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class SalesMap
    {
        public SalesMap(EntityTypeBuilder<Sales> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.SalesDate).IsRequired();
            entityTypeBuilder.Property(x => x.SalesCode).IsRequired();
            entityTypeBuilder.Property(x => x.CurrencyRate).HasPrecision(18, 2).IsRequired();
            entityTypeBuilder.Property(x => x.SalesAmount).HasPrecision(18, 2).IsRequired();

            entityTypeBuilder.HasOne(x => x.Package)
                            .WithMany(x => x.Sales)
                            .HasForeignKey(x => x.PackageId);  
            entityTypeBuilder.HasOne(s => s.RentACarPrice)
                            .WithMany(r => r.Sales)
                            .HasForeignKey(s => s.RentACarPriceId);
            entityTypeBuilder.HasOne(s => s.TourGuidePrice)
                             .WithMany(g => g.Sales)
                             .HasForeignKey(s => s.TourGuidePriceId);
            entityTypeBuilder.HasOne(x => x.Customer)
                            .WithOne(x => x.Sales)
                            .HasForeignKey<Customer>(x => x.SalesId);
        }
    }
}
