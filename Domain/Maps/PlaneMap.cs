using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class PlaneMap
    {
        public PlaneMap(EntityTypeBuilder<Plane> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Brand).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Model).IsRequired().HasMaxLength(50);        
        }
    }
}
