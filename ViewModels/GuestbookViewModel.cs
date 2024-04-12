using MemberSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberSystem.ViewModels
{
    public class GuestbookViewModel
    {
        //顯示資料陣列
        public List<Guestbook> DataList { get; set; }
    }
}