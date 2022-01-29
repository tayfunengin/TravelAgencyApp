using Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Status = Convert.ToBoolean(Domain.Enums.Status.Active);
            AddedDate = DateTime.Now;
            CreatedIpAddress = IpAddresses.GetHostName();
        }
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }     
        public string CreatedIpAddress { get; set; }
        public bool Status { get; set; }
    }
}
