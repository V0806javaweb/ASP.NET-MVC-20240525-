using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MemberSystem.Models
{
    public class Item
    {
        //商品編號
        [DisplayName("商品編號")]
        public int Id { get; set; }
        //商品名稱
        [DisplayName("商品名稱")]
        [Required(ErrorMessage ="輸入商品名稱")]
        [StringLength(50,ErrorMessage ="最長50字")]
        public string Name { get; set; }
        //商品價格
        [DisplayName("商品價格")]
        [Required(ErrorMessage ="輸入價格")]
        public int Price { get; set; }
        //商品圖片
        [DisplayName("圖片")]
        public string Image { get; set; }
    }
}