using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberSystem.Models
{
    public class CartSave
    {
        //會員帳號
        public string Account { get; set; }
        //資料編號
        public string Cart_Id { get; set; }
        //外部參考
        public Member Member { get; set; }
    }
}