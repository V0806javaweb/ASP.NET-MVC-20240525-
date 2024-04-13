using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MemberSystem.Models
{
    public class Guestbook
    {
        //編號
        [DisplayName("編號: ")]
        public int Id { get; set; }
        //名字
        [DisplayName("名字: ")]
        [Required(ErrorMessage ="不接受匿名")]
        [StringLength(20,ErrorMessage ="最多20字")]
        public string Name { get; set; }
        //留言內容
        [DisplayName("留言內容: ")]
        [Required(ErrorMessage ="說話")]
        [StringLength(100,ErrorMessage ="話太多了只能100字")]
        public string Content { get; set; }
        //新增時間
        [DisplayName("新增時間")]
        public DateTime CreateTime { get; set; }
        //回覆內容
        [DisplayName("回覆內容: ")]
        [StringLength(100,ErrorMessage ="精簡點100字")]
        public string Reply { get; set; }
        //回復時間
        //DateTime?資料型態，允許DateTime有NULL產生
        [DisplayName("回復時間: ")]
        public DateTime? ReplyTime { get; set; }
    }
}