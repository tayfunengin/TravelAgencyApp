using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class TourDateInPackageMap
    {
        public TourDateInPackageMap(EntityTypeBuilder<TourDateInPackage> entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(x => x.Package)
                            .WithMany(p => p.TourDates)
                            .HasForeignKey(x => x.PackageId);

            entityTypeBuilder.HasOne(x => x.TourPrice)
                            .WithMany(p => p.TourDates)
                            .HasForeignKey(x => x.TourPriceId);
           
        }
    }
}
