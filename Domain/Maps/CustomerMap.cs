using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class CustomerMap
    {
        public CustomerMap(EntityTypeBuilder<Customer> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Surname).IsRequired().HasMaxLength(50);  
            entityTypeBuilder.Property(x => x.Email).IsRequired().HasMaxLength(150);

            entityTypeBuilder.HasOne(x => x.Sales)
                            .WithOne(x => x.Customer)
                            .HasForeignKey<Sales>(x => x.CustomerId);
        }
    }
}
