using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class TransportationCompanyMap
    {
        public TransportationCompanyMap(EntityTypeBuilder<TransportationCompany> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.TransportaionCompanyCode).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.TransportationCompanyName).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Address).IsRequired().HasMaxLength(250);          
            entityTypeBuilder.HasOne(x => x.Region).WithMany(x => x.TransportationCompanies).HasForeignKey(x => x.RegionId);

        }
    }
}
