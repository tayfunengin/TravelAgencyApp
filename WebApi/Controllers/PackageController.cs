using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Auth;
using WebApi.Tool;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BasicAuth(_loginSecurity)]
    public class PackageController : ControllerBase
    {
        private readonly ILoginSecurity _loginSecurity;

        public PackageController(ILoginSecurity loginSecurity)
        {
            this._loginSecurity = loginSecurity;
        }
    }
}
