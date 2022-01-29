using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class RentACarCompany : BaseEntity
    {
        [Required(ErrorMessage = "RentACarCompanyCode is required.")]
        [MaxLength(50, ErrorMessage = "RentACarCompanyCode can have max 50 character.")]
        public string RentACarCompanyCode { get; set; }

        [Required(ErrorMessage = "RentACarCompanyName is required.")]
        [MaxLength(50, ErrorMessage = "RentACarCompanyName can have max 50 character.")]
        public string RentACarCompanyName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250, ErrorMessage = "Address can have max 250 character.")]
        public string Address { get; set; }
        public virtual List<Car> Cars { get; set; }

        public virtual List<RentACarPrice> Prices { get; set; }
   
    }
}
