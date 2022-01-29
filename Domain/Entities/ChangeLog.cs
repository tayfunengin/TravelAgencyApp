using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ChangeLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ComputerName { get; set; }
        public string IpAddress { get; set; }
        public DateTime ModifiedDate { get; set; }   
        public string EntityName{ get; set; }
        public int EntityId { get; set; }
        public string PropertyName { get; set; }
        public string PreviousValue { get; set; }
        public string NextValue { get; set; }
    }
}
