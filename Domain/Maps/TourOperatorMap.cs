using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class TourOperatorMap
    {
        public TourOperatorMap(EntityTypeBuilder<TourOperator> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.TourOperatorCode).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.TourOperatorName).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Address).IsRequired().HasMaxLength(250);
            
            
        }
    }
}
