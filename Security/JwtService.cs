using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace MemberSystem.Security
{
    public class JwtService
    {
        #region 製作Token
        public string GenerateToken(string Account,string Role)
        {
            JwtObject jwtobject = new JwtObject
            {
                Account = Account,
                Role = Role,
                Expire = DateTime.Now.AddMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["ExpireMinutes"])).ToString()
            };
            //get key from web.config
            string SecretKey = WebConfigurationManager.AppSettings["SecretKey"].ToString();
            //JWT content
            var payload = jwtobject;
            var token = JWT.Encode(payload, Encoding.UTF8.GetBytes(SecretKey), JwsAlgorithm.HS512);
            return token;
        }
        #endregion
    }
}