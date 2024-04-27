using Jose;
using MemberSystem.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MemberSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_OnPostAuthenticateRequest(object sender,EventArgs e)
        {
            //take request and set jwt key cookiename
            HttpRequest httpRequest = HttpContext.Current.Request;
            string SecretKey = WebConfigurationManager.AppSettings["SecretKey"].ToString();
            string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();

            //檢查cookie 內是否有 token
            if (httpRequest.Cookies[cookieName] != null)
            {
                //還原token
                JwtObject jwtObject = JWT.Decode<JwtObject>
                    (Convert.ToString(httpRequest.Cookies[cookieName].Value),Encoding.UTF8.GetBytes(SecretKey), JwsAlgorithm.HS512);

                //取資料做陣列
                string[] roles = jwtObject.Role.Split(new char[] { ',' });
                //自建取代httpcontext.current.user
                Claim[] claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,jwtObject.Account),
                    new Claim(ClaimTypes.NameIdentifier,jwtObject.Account)
                };

                var claimsIdentity = new ClaimsIdentity(claims,cookieName);

                //for @Html.AntiForgeryToken()  (也許不用)
                claimsIdentity.AddClaim(new Claim
                    (@"http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "My Identity",
                    @"http://www.w3.org/2001/XMLSchema#string"));

                //指派身分權限到HttpContext 中 user
                HttpContext.Current.User = new GenericPrincipal(claimsIdentity, roles);
                Thread.CurrentPrincipal = HttpContext.Current.User;
            }
        }
    }
}
