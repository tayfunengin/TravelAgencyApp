using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class RoomTypeAndHotel : BaseEntity
    {
        public int RoomTypeId { get; set; }
        public int HotelId { get; set; }
        public virtual RoomType RoomType { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
