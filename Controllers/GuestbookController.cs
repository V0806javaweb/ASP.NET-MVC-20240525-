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

        #region 修改留言
        public ActionResult Edit(int Id)
        {
            Guestbook Data = GuestbookService.GetDataById(Id);
            return View(Data);
        }
        //action for 修改留言傳入資料
        [HttpPost]
        public ActionResult Edit(int Id,[Bind(Include ="Name,Content")]Guestbook UpdateData)
        {
            //use check method prevent update error
            if (GuestbookService.CheckUpdate(Id))
            {
                UpdateData.Id = Id;
                GuestbookService.UpdateGuestbook(UpdateData);
                //return RedirectToAction("Index");
            }
            /*else
            {
                return RedirectToAction("Index");
            }*/
            return RedirectToAction("Index");
        }
        #endregion

        #region 回覆留言
        public ActionResult Replpy(int Id)
        {
            Guestbook Data = GuestbookService.GetDataById(Id);
            return View(Data);
        }

        //modify action
        [HttpPost]
        public ActionResult Reply(int Id,[Bind(Include ="Reply,ReplyTime")]Guestbook ReplyData)
        {
            if (GuestbookService.CheckUpdate(Id))
            {
                ReplyData.Id = Id;
                GuestbookService.ReplyGuestbook(ReplyData);
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}