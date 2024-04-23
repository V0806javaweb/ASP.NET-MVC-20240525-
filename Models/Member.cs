using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberSystem.Models
{
    public class Member
    {
        //帳號
        [DisplayName("帳號")]
        [Required(ErrorMessage = "輸入帳號")]
        [StringLength(30,MinimumLength = 6,ErrorMessage = "長度需於6~30字元")]
        [Remote("AccountCheck","Member",ErrorMessage = "帳號已註冊")]
        public string Account { get; set; }
        //密碼
        public string Password { get; set; }
        //姓名
        [DisplayName("姓名")]
        [StringLength(20,ErrorMessage = "姓名至多20字元")]
        [Required(ErrorMessage = "輸入姓名")]
        public string Name { get; set; }
        //電郵
        [DisplayName("Email")]
        [Required(ErrorMessage = "輸入Email")]
        [StringLength(200,ErrorMessage = "電郵長度200內")]
        [EmailAddress(ErrorMessage = "電郵格式錯誤")]
        public string Email { get; set; }
        //信箱驗證
        public string AuthCode { get; set; }
        //帳號身分
        public bool IsAdmin { get; set; }
    }
}