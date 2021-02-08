using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace EmpoyeeServiceAPI
{
    public class BasicAuthenticationAttribute:AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
           if( actionContext.Request.Headers.Authorization == null)
            {
                 actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized );
            }
           else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                //here we are fetching username and password from request header , since it is basic authentication so username and pasword will be encocode i base 64 format 
                //so we are decoding and fetching username and password , Username:password
                string decodedauthenticationToken = Encoding.UTF8.GetString( Convert.FromBase64String(authenticationToken));
                string [] userNamePasswordarray = decodedauthenticationToken.Split(':');
                string userName = userNamePasswordarray[0];
                string password = userNamePasswordarray[1];

                if(EmployeesSecurity.Login(userName,password ))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

            }
                
        }
    }
}