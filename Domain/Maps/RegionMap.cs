using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class RegionMap
    {
        public RegionMap(EntityTypeBuilder<Region> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.RegionName).IsRequired().HasMaxLength(50);            
        }
    }
}
