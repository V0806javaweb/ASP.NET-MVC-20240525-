using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberSystem.Services
{
    public class ForPaging
    {
        //current page
        public int NowPage { get; set; }
        //maximum page
        public int MaxPage { get; set; }
        //手動指定1頁多少筆資料
        //唯獨
        public int ItemNum { get { return 5; } }

        //default constructor
        public ForPaging()
        {
            this.NowPage = 1;
        }

        //指定頁數
        public ForPaging(int Page)
        {
            this.NowPage = Page;
        }

        //格式驗證
        public void SetRightPage()
        {
            //最小從1開始
            if (this.NowPage < 1)
            {
                this.NowPage = 1;
            }
            //超過設最大
            else if (this.NowPage > this.MaxPage)
            {
                this.NowPage = this.MaxPage;
            }
            if (this.MaxPage.Equals(0))
            {
                this.NowPage = 1;
            }
        }
    }
}