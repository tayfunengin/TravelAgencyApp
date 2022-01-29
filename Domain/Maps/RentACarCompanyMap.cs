using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class RentACarCompanyMap
    {
        public RentACarCompanyMap(EntityTypeBuilder<RentACarCompany> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.RentACarCompanyCode).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.RentACarCompanyName).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Address).IsRequired().HasMaxLength(250);
        }
    }
}
