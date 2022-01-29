using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class TourOperator : BaseEntity
    {
        [Required(ErrorMessage = "TourOperatorCode is required.")]
        [MaxLength(50, ErrorMessage = "TourOperatorCode can have max 50 character.")]
        public string TourOperatorCode { get; set; }

        [Required(ErrorMessage = "TourOperatorName is required.")]
        [MaxLength(50, ErrorMessage = "TourOperatorName can have max 50 character.")]
        public string TourOperatorName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250, ErrorMessage = "Address can have max 250 character.")]
        public string Address { get; set; }
        public virtual List<Tour> Tours { get; set; }
        public virtual List<TourOperatorPrice> Prices { get; set; }   
    }
}
