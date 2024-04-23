using MemberSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MemberSystem.Services
{
    public class MemberDBService
    {
        //DB connect 
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["MemberDBEntities"].ConnectionString;
        //active connect
        private readonly SqlConnection conn = new SqlConnection(cnstr);

        #region 註冊
        public void Register(Member newMember)
        {
            newMember.Password = HashPassword(newMember.Password);
            string sql = $@"INSERT INTO Members (Account,Password,Name,Email,AuthCode,IsAdmin) VALUES ('{newMember.Account}','{newMember.Password}','{newMember.Name}','{newMember.Email}','{newMember.AuthCode}','0')";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region 密碼-哈希
        public string HashPassword(string Password)
        {
            string saltkey = "1q2w3e4r5t6y7u8i9o0po7tyy";
            //password + salt
            string saltAndPassword = String.Concat( Password,saltkey);
            //加密物件
            SHA256CryptoServiceProvider sha256Hasher = new SHA256CryptoServiceProvider();
            //convert Password+salt string
            byte[] PasswordData = Encoding.Default.GetBytes(saltAndPassword);
            //hash the byte data
            byte[] HashData = sha256Hasher.ComputeHash(PasswordData);
            //convert result to string
            string Hashresult = Convert.ToBase64String(HashData);
            return Hashresult;
        }
        #endregion

        #region 以帳號查詢
        private Member GetDataByAccount(string Account)
        {
            Member Data = new Member();
            string sql = $@"select * from Members where Account = '{Account}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Password = dr["Password"].ToString();
                Data.Name = dr["Name"].ToString();
                Data.Email = dr["Email"].ToString();
                Data.AuthCode = dr["AuthCode"].ToString();
                Data.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
            }
            catch (Exception e)
            {
                //not found
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        #endregion

        #region 帳號唯一性確認
        public bool AccountCheck(string Account)
        {
            Member Data = GetDataByAccount(Account);
            return !(Data == null);
        }
        #endregion

        #region 信箱驗證
        public string EmailValidate(string Account, string AuthCode)
        {
            Member ValidateMember = GetDataByAccount(Account);
            string ValidateStr = string.Empty; ;
            if(ValidateMember!= null)
            {
                if(ValidateMember.AuthCode == AuthCode)
                {
                    string sql = $@"update Members set AuthCode = '{string.Empty}' where Account = '{Account}'";
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }
                    ValidateStr = "驗證完畢，可登入";
                }
                else
                {
                    ValidateStr = "驗證碼錯誤";
                }
            }
            else
            {
                ValidateStr = "無此帳號";
            }
            return ValidateStr;
        }
        #endregion
    }
}