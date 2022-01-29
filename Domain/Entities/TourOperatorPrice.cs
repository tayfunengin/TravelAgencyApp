using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class TourOperatorPrice : BaseEntity
    {
        [Required(ErrorMessage = "Purchase price is required.")]
        public decimal PurchasePrice { get; set; }
        [Required(ErrorMessage = "Sale price is required.")]
        public decimal SalePrice { get; set; }
        [Required(ErrorMessage = "Period is required.")]
        public Period Period { get; set; }
        [Required(ErrorMessage = "Year is required.")]
        public string Year { get; set; } 
        public int TourOperatorId { get; set; }
        public virtual TourOperator TourOperator { get; set; }
        public int TourId { get; set; }
        public virtual Tour Tour { get; set; }
        public virtual List<TourDateInPackage> TourDates { get; set; }
        public virtual List<Package> Packages { get; set; }
    }
}
