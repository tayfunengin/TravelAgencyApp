using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class AccountBalanceMap
    {
        public AccountBalanceMap(EntityTypeBuilder<AccountBalance> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.SellerId).IsRequired();
            entityTypeBuilder.Property(x => x.CurrentDept).IsRequired().HasPrecision(18, 2);
            entityTypeBuilder.Property(x => x.CurrentCredit).IsRequired().HasPrecision(18, 2);           

            entityTypeBuilder.HasOne(x => x.Sales)
                            .WithMany(x => x.AccountBalances)
                            .HasForeignKey(x => x.SalesId);

        }
    }
}
