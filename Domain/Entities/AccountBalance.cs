using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AccountBalance : BaseEntity
    {
        public int SellerId { get; set; }
        public string AccountName { get; set; }
        public decimal CurrentDept  { get; set; }
        public decimal CurrentCredit { get; set; }   
      //  public decimal CurrentBalance { get; set; }
        public DateTime Date { get; set; }
        public int SalesId { get; set; }
        public virtual Sales Sales { get; set; }
    }

}
