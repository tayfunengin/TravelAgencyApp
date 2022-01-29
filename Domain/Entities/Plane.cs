using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Plane : BaseEntity
    {
        [Required(ErrorMessage = "Brand is required.")]
        [MaxLength(50, ErrorMessage = "Brand can have max 50 character.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        [MaxLength(50, ErrorMessage = "Model can have max 50 character.")]
        public string Model { get; set; }
        public virtual List<FlightCompany> FlightCompanies { get; set; }
    }
}
