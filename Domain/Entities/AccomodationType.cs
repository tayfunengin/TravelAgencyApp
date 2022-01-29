using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class AccomodationType : BaseEntity
    {
        [Required(ErrorMessage = "AccomodationCode is required.")]
        [MaxLength(50, ErrorMessage = "AccomodationCode can have max 50 character.")]
        public string AccomodationCode { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(100, ErrorMessage = "Description can have max 100 character.")]
        public string Description { get; set; }       
        public virtual List<Hotel> Hotels { get; set; }
    }
}
