using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class TourMap
    {
        public TourMap(EntityTypeBuilder<Tour> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.TourCode).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.TourName).IsRequired().HasMaxLength(50);
            entityTypeBuilder.HasOne(t => t.Region)
                .WithMany(r => r.Tours).
                HasForeignKey(t => t.RegionId);
            entityTypeBuilder.HasOne(t => t.TourOperator)
                .WithMany(TourOperator => TourOperator.Tours)
                .HasForeignKey(t => t.TourOperatorId);
        }
    }
}
