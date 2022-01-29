using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class UserLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public DateTime LogDate { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string Description { get; set; }
    }
}
