using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class RoomLocationAndHotel : BaseEntity
    {
        public int RoomLocationId { get; set; }
        public int HotelId { get; set; }
        public virtual RoomLocation RoomLocation { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
