using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class RoomLocationAndHotelMap
    {
        public RoomLocationAndHotelMap(EntityTypeBuilder<RoomLocationAndHotel> entityTypeBuilder)
        {
            entityTypeBuilder.Ignore(x => x.Id);
            entityTypeBuilder.HasKey(x => new { x.RoomLocationId, x.HotelId });
        }
    }
}
