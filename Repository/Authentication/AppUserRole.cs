using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Authentication
{
    public class AppUserRole : IdentityRole<int>
    {
        public string Description { get; set; }
    }
}
