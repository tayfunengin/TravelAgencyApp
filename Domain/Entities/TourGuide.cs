using Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class TourGuide : BaseEntity
    {
        [Required(ErrorMessage = "TourGuideCode is required.")]
        [MaxLength(50, ErrorMessage = "TourGuideCode can have max 50 character.")]
        public string TourGuideCode { get; set; }

        [Required(ErrorMessage = "TourGuideName is required.")]
        [MaxLength(50, ErrorMessage = "TourGuideName can have max 50 character.")]
        public string TourGuideName { get; set; }

        [Required(ErrorMessage = "TourGuideSurname is required.")]
        [MaxLength(50, ErrorMessage = "TourGuideSurname can have max 50 character.")]
        public string TourGuideSurname { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250, ErrorMessage = "Address can have max 250 character.")]
        public string Address { get; set; }
        public Language Language { get; set; }

        public int RegionId { get; set; }
        public virtual Region Region { get; set; }
        public virtual List<TourGuidePrice> Prices { get; set; }       
    }
}
