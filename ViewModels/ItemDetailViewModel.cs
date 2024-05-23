using MemberSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberSystem.ViewModels
{
    public class ItemDetailViewModel
    {
        //新增商品內容
        public Item Data { get; set; }
        //是否於購物車內
        public bool InCart { get; set; }
    }
}