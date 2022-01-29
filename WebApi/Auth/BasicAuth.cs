using Microsoft.AspNetCore.Identity;
using Repository.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApi.Tool;

namespace WebApi.Auth
{
    public class BasicAuth : AuthorizationFilterAttribute
    {
        private readonly ILoginSecurity loginSecurity;

        public BasicAuth(ILoginSecurity loginSecurity)
        {
            this.loginSecurity = loginSecurity;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {            
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
         
                string decodeAuthToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));      

                string[] userNameAndPasswordArray = decodeAuthToken.Split(':');
                string userName = userNameAndPasswordArray[0];
                string password = userNameAndPasswordArray[1];

                          
                
                if (loginSecurity.Login(userName, password))
                {                 
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                }        

            }
        }
    }
}
