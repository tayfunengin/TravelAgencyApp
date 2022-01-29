using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class HotelMap
    {
        public HotelMap(EntityTypeBuilder<Hotel> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.HotelCode).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.HotelName).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Address).IsRequired().HasMaxLength(250);

            entityTypeBuilder.HasOne(h => h.Region)
             .WithMany(r => r.Hotels).
             HasForeignKey(t => t.RegionId);
        }
    }
}
