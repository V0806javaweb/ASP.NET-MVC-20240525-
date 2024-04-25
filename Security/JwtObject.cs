using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberSystem.Security
{
    public class JwtObject
    {
        //要設定到期時間 可被對稱解密
        public string Account { get; set; }
        public string Role { get; set; }
        public string Expire { get; set; }
    }
}