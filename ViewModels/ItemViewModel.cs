using MemberSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberSystem.ViewModels
{
    public class ItemViewModel
    {
        //資料
        public List<int> IdList { get; set; }
        //商品
        public List<ItemDetailViewModel> ItemBlock { get; set; }
        //分頁
        public ForPaging Paging { get; set; }
    }
}