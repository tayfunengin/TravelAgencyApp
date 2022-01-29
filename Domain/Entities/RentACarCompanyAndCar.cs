using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class RentACarCompanyAndCar : BaseEntity
    {
        public int RentACarCompanyId { get; set; }
        public int CarId { get; set; }

        public RentACarCompany RentACarCompany { get; set; }
        public Car Car { get; set; }
    }
}
