using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberSystem.Models
{
    public class Guestbook
    {
        //編號
        public int Id { get; set; }
        //名字
        public string Name { get; set; }
        //留言內容
        public string Content { get; set; }
        //新增時間
        public DateTime CreateTime { get; set; }
        //回覆內容
        public string Reply { get; set; }
        //回復時間
        //DateTime?資料型態，允許DateTime有NULL產生
        public DateTime? ReplyTime { get; set; }
    }
}