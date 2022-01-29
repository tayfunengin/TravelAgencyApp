using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Customer : BaseEntity
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required, EmailAddress]   
        public string Email { get; set; }
        public int? SalesId { get; set; }
        public virtual Sales Sales { get; set; }
    }
}
