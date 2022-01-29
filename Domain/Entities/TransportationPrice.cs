using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class TransportationPrice : BaseEntity
    {
        [Required(ErrorMessage = "Purchase price is required.")]
        public decimal PurchasePrice { get; set; }
        [Required(ErrorMessage = "Sale price is required.")]
        public decimal SalePrice { get; set; }
        [Required(ErrorMessage = "Period is required.")]
        public Period Period { get; set; }
        [Required(ErrorMessage = "Year is required.")]
        public string Year { get; set; }

        public int TransportationCompanyId { get; set; }
        public virtual TransportationCompany TransportationCompany { get; set; }

        public int RouteId { get; set; }
        public virtual Route Route { get; set; }

        public virtual List<Package> Packages { get; set; }
    }
}
