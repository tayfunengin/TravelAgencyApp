    using Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Hotel : BaseEntity
    {
        [Required(ErrorMessage = "HotelCode is required.")]
        [MaxLength(50, ErrorMessage = "HotelCode can have max 50 character.")]
        public string HotelCode { get; set; }

        [Required(ErrorMessage = "HotelName is required.")]
        [MaxLength(50, ErrorMessage = "HotelName can have max 50 character.")]
        public string HotelName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250, ErrorMessage = "Address can have max 250 character.")]
        public string Address { get; set; }
        public string ProfilePhotoPath { get; set; }

        public int RegionId { get; set; }
        public virtual Region Region { get; set; }
        public virtual List<AccomodationType>  AccomodationTypes { get; set; }
        public virtual List<RoomType> RoomTypes { get; set; }
        public virtual List<RoomLocation> RoomLocations { get; set; }
        public virtual List<Photography> Photographies { get; set; }
        public virtual List<HotelPrice> Prices { get; set; }        
    }
}
