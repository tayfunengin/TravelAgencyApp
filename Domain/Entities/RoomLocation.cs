using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class RoomLocation : BaseEntity
    {
        [Required(ErrorMessage = "RoomLocationCode is required.")]
        [MaxLength(50, ErrorMessage = "RoomLocationCode can have max 50 character.")]
        public string RoomLocationCode { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(100, ErrorMessage = "Description can have max 100 character.")]
        public string Description { get; set; }
        public virtual List<Hotel> Hotels { get; set; }
    }
}
