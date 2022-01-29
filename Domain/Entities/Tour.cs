using Common;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Tour : BaseEntity
    {
        [Required(ErrorMessage = "TourCode is required")]
        [MaxLength(50, ErrorMessage = "TourCode can have max 50 character.")]
        public string TourCode { get; set; }

        [Required(ErrorMessage = "TourName is required")]
        [MaxLength(50, ErrorMessage = "TourName can have max 50 character.")]
        public string TourName { get; set; }     
        public int RegionId { get; set; }
        public virtual Region Region { get; set; }
        public int TourOperatorId { get; set; }
        public virtual TourOperator TourOperator { get; set; }

        public virtual List<TourOperatorPrice> TourOperatorPrices { get; set; }
    }

}
