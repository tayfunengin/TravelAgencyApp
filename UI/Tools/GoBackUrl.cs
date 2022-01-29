using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;



namespace UI.Tools
{
    public class GoBackUrl
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public GoBackUrl(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string GetBackUrl()
        {
          return  httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
        }
    }
}
