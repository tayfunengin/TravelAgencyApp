using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class RoomLocationMap
    {
        public RoomLocationMap(EntityTypeBuilder<RoomLocation> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.RoomLocationCode).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Description).IsRequired().HasMaxLength(100);

        }
    }
}
