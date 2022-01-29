using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Region : BaseEntity
    {
        [Required(ErrorMessage = "RegionName is required.")]
        [MaxLength(50, ErrorMessage = "RegionName can have max 50 character.")]
        public string RegionName { get; set; }

        public virtual List<Hotel> Hotels { get; set; }
        public virtual List<Tour> Tours { get; set; }

        public virtual List<TourGuide> TourGuides { get; set; } 
        public virtual List<FlightCompany> FlightCompanies { get; set; }
        public virtual List<TransportationCompany> TransportationCompanies { get; set; }

        public virtual List<Package> Packages { get; set; }
    }
}
