using Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Route : BaseEntity
    {
        [Required(ErrorMessage = "RouteCode is required.")]
        [MaxLength(50, ErrorMessage = "RouteCode can have max 50 character.")]
        public string RouteCode { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(100, ErrorMessage = "Description can have max 100 character.")]
        public string Description { get; set; }   
        public virtual List<TransportationCompany> TransportationCompanies { get; set; }
        public virtual List<FlightCompany> FlightCompanies { get; set; }

    }
}
