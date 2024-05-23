using MemberSystem.Services;
using MemberSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberSystem.Controllers
{
    public class ItemController : Controller
    {
        //Cart service object
        private readonly CartService cartService = new CartService();
        //Item service object
        private readonly ItemService itemService = new ItemService();

        public ActionResult Index(int Page = 1)
        {
            //define page model
            ItemViewModel Data = new ItemViewModel();
            Data.Paging = new ForPaging(Page);
            //get data array from service
            Data.IdList = itemService.GetIdList(Data.Paging);
            Data.ItemBlock = new List<ItemDetailViewModel>();
            foreach(var Id in Data.IdList)
            {
                //define object in array 
                ItemDetailViewModel newBlock = new ItemDetailViewModel();
                newBlock.Data = itemService.GetDataById(Id);
                string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
                newBlock.InCart = cartService.CheckInCart(Cart, Id);
                Data.ItemBlock.Add(newBlock);
            }
            return View(Data);
        }

        #region 商品頁面
        public ActionResult Item(int Id)
        {
            ItemDetailViewModel ViewData = new ItemDetailViewModel();
            ViewData.Data = itemService.GetDataById(Id);//商品資料
            //用戶個人購物車資料
            string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
            //商品是否被加入購物車
            ViewData.InCart = cartService.CheckInCart(Cart, Id);
            return View(ViewData);
        }
        #endregion

        #region 商品列表版面
        public ActionResult ItemBlock(int Id)
        {
            ItemDetailViewModel ViewData = new ItemDetailViewModel();
            ViewData.Data = itemService.GetDataById(Id);
            string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
            ViewData.InCart = cartService.CheckInCart(Cart,Id);
            return PartialView(ViewData);
        }
        #endregion

        #region 新增商品
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Add(ItemCreateViewModel Data)
        {
            if (Data.ItemImage != null)
            {//商品必須有預覽圖
                string filename = Path.GetFileName(Data.ItemImage.FileName);        //get file name
                string Url = Path.Combine(Server.MapPath("~/Upload/"), filename);   //合併伺服器路徑與檔名
                Data.ItemImage.SaveAs(Url);                                         //save at server
                Data.NewData.Image = filename;
                itemService.Insert(Data.NewData);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("ItemImage", "選擇上傳檔案");
                return View(Data);
            }
        }
        #endregion

        #region 刪除商品
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int Id)
        {
            itemService.Delete(Id);             //call service method
            return RedirectToAction("Index");
        }
        #endregion
    }
}