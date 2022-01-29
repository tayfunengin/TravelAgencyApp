using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class TourGuidePrice : BaseEntity
    {
        [Required(ErrorMessage = "Purchase price is required.")]
        public decimal PurchasePrice { get; set; }
        [Required(ErrorMessage = "Sale price is required.")]
        public decimal SalePrice { get; set; }
        [Required(ErrorMessage = "Period is required.")]
        public Period Period { get; set; }
        [Required(ErrorMessage = "Year is required.")]
        public string Year { get; set; }

        public int TourGuideId { get; set; }
        public virtual TourGuide TourGuide { get; set; }

        public virtual List<Package> Packages { get; set; }

        public virtual List<Sales> Sales { get; set; }
    }
}
