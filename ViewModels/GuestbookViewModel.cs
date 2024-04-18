using MemberSystem.Models;
using MemberSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MemberSystem.ViewModels
{
    public class GuestbookViewModel
    {
        //user search scope
        [DisplayName("搜尋: ")]
        public string Search { get; set; }

        //顯示資料陣列
        public List<Guestbook> DataList { get; set; }

        //paging
        [DisplayName("搜尋: ")]
        public ForPaging Paging{ get; set; }

    }
}