using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MemberSystem.Services
{
    public class MailService
    {
        private string gmail_account = "u10806156@ms.ttu.edu.tw";
        private string gmail_password = "ndrq vanh peqc sdxy";
        //private string gmail_mail = "";

        #region 寄送驗證信
        //生成驗證碼
        public string GetValidateCode()
        {
            string[] Code = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            //initialize
            string ValidateCode = string.Empty;
            Random rd = new Random();
            for(int i = 0; i < 10; i++)
            {
                ValidateCode += Code[rd.Next(Code.Count())];
            }
            return ValidateCode;
        }

        //用戶填寫資料引入驗證信
        public string GetRegisterMailBody(string TempString,string UserName,string ValidateUrl)
        {
            TempString = TempString.Replace("{{UserName}}", UserName);
            TempString = TempString.Replace("{{ValidateUrl}}", ValidateUrl);
            return TempString;
        }

        //寄驗證信
        public void SendRegisterMail(string MailBody,string ToEmail)
        {
            //smtp object using gmail
            SmtpClient SmtpServer = new SmtpClient("stmp.gmail.com");
            //port for gmail
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(gmail_account, gmail_password);
            SmtpServer.EnableSsl = true;
            
            //設定信件內容
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(gmail_account);
            mail.To.Add(ToEmail);
            mail.Subject = "註冊驗證信";
            mail.Body = MailBody;
            mail.IsBodyHtml = true; //HTML格式
            SmtpServer.Send(mail);
        }
        #endregion
    }
}