using MemberSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MemberSystem.ViewModels
{
    public class MemberRegisterViewModel
    {
        public Member newMember { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage ="輸入密碼")]
        public string Password { get; set; }

        [DisplayName("確認密碼")]
        [Compare("Password", ErrorMessage = "兩次輸入密碼不一致")]
        [Required(ErrorMessage = "再次輸入密碼")]
        public string PasswordCheck { get; set; }
    }
}