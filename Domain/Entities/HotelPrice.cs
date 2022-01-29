using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class HotelPrice : BaseEntity
    {       
        [Required(ErrorMessage ="Alış fiyatı zorunlu alan.")]
        public decimal PurchasePrice { get; set; }
        [Required(ErrorMessage = "Satış fiyatı zorunlu alan.")]
        public decimal SalePrice { get; set; }
        [Required(ErrorMessage = "Dönem zorunlu alan.")]
        public Period Period { get; set; }
     
        [Required]
        public string Year { get; set; }       
        public int HotelId { get; set; }        
        public virtual Hotel Hotel { get; set; }
        [Required(ErrorMessage = "Konaklama tipi zorunlu alan.")]
        public int AccomodationTypeId { get; set; }
        public virtual AccomodationType AccomodationType { get; set; }
        [Required(ErrorMessage = "Oda konumu zorunlu alan.")]
        public int RoomLocationId { get; set; }
        public virtual RoomLocation RoomLocation { get; set; }
        [Required(ErrorMessage = "Oda tipi zorunlu alan.")]
        public int RoomTypeId { get; set; }
        public virtual RoomType RoomType { get; set; }

        public virtual List<Package> Packages { get; set; }

    }
}
