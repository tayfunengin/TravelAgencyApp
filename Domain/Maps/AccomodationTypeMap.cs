using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class AccomodationTypeMap
    {
        public AccomodationTypeMap(EntityTypeBuilder<AccomodationType> entityBuilderType)
        {
            entityBuilderType.HasKey(x => x.Id);
            entityBuilderType.Property(x => x.AccomodationCode).IsRequired().HasMaxLength(50);
            entityBuilderType.Property(x => x.Description).IsRequired().HasMaxLength(100);          
        }
    }
}
