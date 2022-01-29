using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Maps
{
    public class RoomTypeAndHotelMap
    {
        public RoomTypeAndHotelMap(EntityTypeBuilder<RoomTypeAndHotel> entityTypeBuilder)
        {
            entityTypeBuilder.Ignore(x => x.Id);
            entityTypeBuilder.HasKey(x => new { x.RoomTypeId, x.HotelId });
        }
    }
}
