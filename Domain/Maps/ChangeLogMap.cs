using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class ChangeLogMap
    {
        public ChangeLogMap(EntityTypeBuilder<ChangeLog> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.UserName).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.ComputerName).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.IpAddress).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.EntityName).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.PropertyName).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.PreviousValue).HasMaxLength(255);
            entityTypeBuilder.Property(x => x.NextValue).HasMaxLength(255);
        }
    }
}
