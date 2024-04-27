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

            //�ˬdcookie ���O�_�� token
            if (httpRequest.Cookies[cookieName] != null)
            {
                //�٭�token
                JwtObject jwtObject = JWT.Decode<JwtObject>
                    (Convert.ToString(httpRequest.Cookies[cookieName].Value),Encoding.UTF8.GetBytes(SecretKey), JwsAlgorithm.HS512);

                //����ư��}�C
                string[] roles = jwtObject.Role.Split(new char[] { ',' });
                //�۫ب��Nhttpcontext.current.user
                Claim[] claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,jwtObject.Account),
                    new Claim(ClaimTypes.NameIdentifier,jwtObject.Account)
                };

                var claimsIdentity = new ClaimsIdentity(claims,cookieName);

                //for @Html.AntiForgeryToken()  (�]�\����)
                claimsIdentity.AddClaim(new Claim
                    (@"http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "My Identity",
                    @"http://www.w3.org/2001/XMLSchema#string"));

                //���������v����HttpContext �� user
                HttpContext.Current.User = new GenericPrincipal(claimsIdentity, roles);
                Thread.CurrentPrincipal = HttpContext.Current.User;
            }
        }
    }
}
