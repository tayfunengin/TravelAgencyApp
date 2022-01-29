using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class PackageMap
    {
        public PackageMap(EntityTypeBuilder<Package> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.CheckOutDate).IsRequired();
            entityTypeBuilder.Property(x => x.CheckInDate).IsRequired();
            entityTypeBuilder.Property(x => x.Quota).IsRequired();
            entityTypeBuilder.Property(x => x.GuestCount).IsRequired();

            entityTypeBuilder.HasOne(p => p.HotelPrice)
                             .WithMany(h => h.Packages)
                             .HasForeignKey(p => p.HotelPriceId);
            entityTypeBuilder.HasMany(p => p.TourPrices)
                             .WithMany(t => t.Packages);                            
            entityTypeBuilder.HasOne(p => p.RentACarPrice)
                            .WithMany(t => t.Packages)
                            .HasForeignKey(p => p.RentACarPriceId);
            entityTypeBuilder.HasMany(p => p.FlightPrices)
                            .WithMany(f => f.Packages);
            entityTypeBuilder.HasMany(p => p.TransportationPrices)
                            .WithMany(t => t.Packages);
            entityTypeBuilder.HasOne(p => p.TourGuidePrice)
                            .WithMany(g => g.Packages)
                            .HasForeignKey(p => p.TourGuidePriceId);
            entityTypeBuilder.HasOne(p => p.Region)
                           .WithMany(r => r.Packages)
                           .HasForeignKey(p => p.RegionId);
        }
    }
}
