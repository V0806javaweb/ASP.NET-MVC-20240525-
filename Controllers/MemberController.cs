using MemberSystem.Services;
using MemberSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberSystem.Controllers
{
    public class MemberController : Controller
    {
        private readonly MemberDBService memberService = new MemberDBService();
        private readonly MailService mailService = new MailService();
        
        public ActionResult Index()
        {
            return View();
        }

        #region 註冊
        //initial page
        public ActionResult Register()
        {
            //確認是否登入
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index","Guestbook");

            return View();
        }

        //傳入註冊資料
        [HttpPost]
        public ActionResult Register(MemberRegisterViewModel RegisterMember)
        {
            //確認已驗證
            if (ModelState.IsValid)
            {
                RegisterMember.newMember.Password = RegisterMember.Password; //代入密碼
                string AuthCode = mailService.GetValidateCode();             //取得驗證碼
                RegisterMember.newMember.AuthCode = AuthCode;
                memberService.Register(RegisterMember.newMember);           //service 進行註冊
                string TempMail = System.IO.File.ReadAllText( Server.MapPath("~/Views/Shared/RegisterEmailTemplate.html"));
                UriBuilder ValidateUrl = new UriBuilder(Request.Url) 
                { 
                    Path = Url.Action("EmailValidate","Member",new { Account = RegisterMember.newMember.Account,AuthCode = AuthCode})
                };

                string MailBody = mailService.GetRegisterMailBody(TempMail, RegisterMember.newMember.Name, ValidateUrl.ToString().Replace("%3F", "?"));
                mailService.SendRegisterMail(MailBody, RegisterMember.newMember.Email);
                TempData["RegisterState"] = "已寄出驗證信，請於電郵查收";
                return RedirectToAction( "RegisterResult");
            }

            //未驗證，清除用戶輸入返回
            RegisterMember.Password = null;
            RegisterMember.PasswordChech = null;
            return View(RegisterMember);
        }

        //顯示結果
        public ActionResult RegisterResult()
        {
            return View();
        }

        //判斷是否重複註冊
        public JsonResult AccountCheck(MemberRegisterViewModel RegisterMember)
        {
            return Json(memberService.AccountCheck(RegisterMember.newMember.Account), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmailValidate(string Account,string AuthCode)
        {
            ViewData["EmailValidate"] = memberService.EmailValidate(Account, AuthCode);
            return View();
        }
        #endregion

        #region 登入
        public ActionResult Login()
        {
            //確認是否成功登入
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index","Guestbook");

            return View();
        }

        [HttpPost]
        public ActionResult Login(MemberLoginViewModel LoginMember)
        {
            string ValidateStr = memberService.LoginCheck(LoginMember.Account, LoginMember.Password);
            if (String.IsNullOrEmpty(ValidateStr))
            {
                //登入成功 進首頁
                return RedirectToAction("Index", "Guestbook");
            }
            else
            {
                //傳回錯誤訊息
                ModelState.AddModelError("",ValidateStr);
                return View(LoginMember);
            }
        }
        #endregion
    }
}