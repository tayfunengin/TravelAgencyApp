using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SalesCodeCounter : BaseEntity
    {     
        public char CodeChar { get; set; }
        public int CodeNumber { get; set; }
    }
}
