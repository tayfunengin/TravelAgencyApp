using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Tool
{
    public interface ILoginSecurity
    {
        bool Login(string username, string password);
    }
}
