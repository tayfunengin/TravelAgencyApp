using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Photography : BaseEntity
    {       
        public string Title { get; set; }

        public string Path { get; set; }

        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
