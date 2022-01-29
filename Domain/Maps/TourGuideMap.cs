using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class TourGuideMap
    {
        public TourGuideMap(EntityTypeBuilder<TourGuide> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.TourGuideCode).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.TourGuideName).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.TourGuideSurname).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Address).IsRequired().HasMaxLength(250);
            entityTypeBuilder.Property(x => x.Language).IsRequired();

            entityTypeBuilder.HasOne(t => t.Region)
             .WithMany(r => r.TourGuides).
             HasForeignKey(t => t.RegionId);
        }
    }
}
