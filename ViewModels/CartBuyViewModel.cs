using MemberSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberSystem.ViewModels
{
    public class CartBuyViewModel
    {
        //購物車內現有商品陣列
        public List<CartBuy> DataList { get; set; }
        //是否保存狀態
        public bool isCartsave { get; set; }
    }
}