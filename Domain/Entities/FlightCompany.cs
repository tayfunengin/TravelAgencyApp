using Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class FlightCompany : BaseEntity
    {
        [Required(ErrorMessage = "FlightCompanyCode is required.")]
        [MaxLength(50, ErrorMessage = "FlightCompanyCode can have max 50 character.")]
        public string FlightCompanyCode { get; set; }

        [Required(ErrorMessage = "FlightCompanyName is required.")]
        [MaxLength(50, ErrorMessage = "FlightCompanyName can have max 50 character.")]
        public string FlightCompanyName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250, ErrorMessage = "Address can have max 250 character.")]
        public string Address { get; set; }
        public virtual List<Plane> Planes { get; set; }
        public virtual List<Region> Regions { get; set; }

        public virtual List<Route> Routes { get; set; }

        public virtual List<FlightPrice> Prices { get; set; }
    
    }
}
