﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberSystem.Models
{
    public class CartBuy
    {
        //購物車編號
        public string Cart_Id { get; set; }
        //商品編號
        public int Item_Id { get; set; }
        //外來參考
        public Item Item { get; set; }
    }
}