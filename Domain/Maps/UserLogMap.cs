using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class UserLogMap
    {
        public UserLogMap(EntityTypeBuilder<UserLog> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.UserName).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Role).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.ActionName).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.ControllerName).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Description).HasMaxLength(255);
        }
    }
}
