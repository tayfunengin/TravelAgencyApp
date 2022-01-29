using Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class TransportationCompany : BaseEntity
    {
        [Required(ErrorMessage = "TransportaionCompanyCode is required.")]
        [MaxLength(50, ErrorMessage = "TransportaionCompanyCode can have max 50 character.")]
        public string TransportaionCompanyCode { get; set; }

        [Required(ErrorMessage = "TransportationCompanyName is required.")]
        [MaxLength(50, ErrorMessage = "TransportationCompanyName can have max 50 character.")]
        public string TransportationCompanyName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250, ErrorMessage = "Address can have max 250 character.")]
        public string Address { get; set; }
        public virtual List<Route> Routes { get; set; }
        public int RegionId { get; set; }
        public virtual Region Region { get; set; }
        public virtual List<TransportationPrice> Prices { get; set; }       
    }
}
