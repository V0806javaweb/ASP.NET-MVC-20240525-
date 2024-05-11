using MemberSystem.Services;
using MemberSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberSystem.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService caerService = new CartService();
        
        [Authorize]
        public ActionResult Index()
        {
            CartBuyViewModel Data = new CartBuyViewModel();
            //load session cart data
            string Cart = (HttpContext.Session["Cart"] != null)? HttpContext.Session["Cart"] .ToString(): null;
            //get cart data by session cart number from service
            Data.DataList = caerService.GetItemFromCart(Cart);

            Data.isCartsave = caerService.CheckCartSave(User.Identity.Name, Cart);
            return View(Data);
        }

        #region 保存購物車
        [Authorize]
        public ActionResult CartSsve()
        {
            string Cart;
            if (HttpContext.Session["Cart"] != null)
            {
                //session有值，寫入剛宣告的購物車物件
                Cart = HttpContext.Session["Cart"].ToString();
            }
            else
            {
                //reset cart
                Cart = DateTime.Now.ToString() + User.Identity.Name;
                //replace session
                HttpContext.Session["Cart"] = Cart;
            }
            caerService.SaveCart(User.Identity.Name, Cart);
            return RedirectToAction("Index");
        }
        #endregion

        #region 取消保存購物車
        [Authorize]
        public ActionResult CartSaveRemove()
        {
            caerService.SaveCartRemove(User.Identity.Name);
            return RedirectToAction("Index");
        }
        #endregion

        #region 從購物車取出商品
        [Authorize]
        public ActionResult Pop(int Id,string toPage)
        {
            string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() :null;
            caerService.RemoveForCart(Cart, Id);
            if (toPage == "Item")
            {
                return RedirectToAction("Item", "Item", new { Id = Id });
            }
            else if (toPage == "ItemBlock")
            {
                return RedirectToAction("ItemBlock", "Item", new { Id = Id });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region 商品放入購物車
        [Authorize]
        public ActionResult Put(int Id,string toPage)
        {
            //若session內尚無用戶購物車紀錄，以使用者名稱、當下時間取名新增購物車
            if (HttpContext.Session["Cart"] == null)
            {
                HttpContext.Session["Cart"] = DateTime.Now.ToString() + User.Identity.Name;
            }
            caerService.AddtoCart(HttpContext.Session["Cart"].ToString(), Id);
            if (toPage == "Item")
            {
                return RedirectToAction("Item", "Item", new { Id = Id });
            }
            else if (toPage == "ItemBlock")
            {
                return RedirectToAction("ItemBlock", "Item", new { Id = Id });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}