using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class PhotographyMap
    {
        public PhotographyMap(EntityTypeBuilder<Photography> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Title).IsRequired().HasMaxLength(50);
            entityTypeBuilder.HasOne(p => p.Hotel)
                .WithMany(h => h.Photographies)
                .HasForeignKey(p => p.HotelId);
        }
    }
}
