using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberSystem.Models
{
    public class Item
    {
        //商品編號
        public int Id { get; set; }
        //商品名稱
        public string Name { get; set; }
        //商品價格
        public int Price { get; set; }
        //商品圖片
        public string Image { get; set; }
    }
}