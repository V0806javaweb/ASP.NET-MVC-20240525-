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
    }
}