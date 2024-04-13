using MemberSystem.Models;
using MemberSystem.Services;
using MemberSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberSystem.Controllers
{
    public class GuestbookController : Controller
    {
        // table Guestbooks service object
        private readonly GuestbookDBService GuestbookService = new GuestbookDBService();

        // GET: Guestbook
        public ActionResult Index()
        {
            //page model
            GuestbookViewModel Data = new GuestbookViewModel();
            //get data from service
            Data.DataList = GuestbookService.GetDataList();
            //deliver Data to viewpage
            return View(Data);
        }

        #region 新增留言
        //add comment's initial page
        public ActionResult Create()
        {
            return PartialView();
        }

        //action about add or sent comment
        //HttpPost only
        //以Bind Include限制接受的欄位值
        [HttpPost]
        public ActionResult Create([Bind(Include ="Name,Content")] Guestbook Data)
        {
            GuestbookService.InsertGuestbook(Data);
            //back to Index page
            return RedirectToAction("Index");
        }
        #endregion
    }
}