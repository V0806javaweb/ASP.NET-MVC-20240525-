using MemberSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MemberSystem.Services
{
    public class CartService
    {
        //DB connect 
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["MemberDB"].ConnectionString;
        //active connect
        private readonly SqlConnection conn = new SqlConnection(cnstr);

        #region 讀取購物車內容物
        public List<CartBuy> GetItemFromCart(string Cart)
        {
            List<CartBuy> DataList = new List<CartBuy>();
            string sql = $@"SELECT * FROM CartBuy m INNER JOIN Item d ON m.Item_Id = d.Id WHERE Cart_Id = '{Cart}';";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CartBuy Data = new CartBuy();
                    Data.Cart_Id = dr["Cart_Id"].ToString();
                    Data.Item_Id = Convert.ToInt32(dr["Item_Id"]);
                    Data.Item.Id = Convert.ToInt32(dr["Id"]);
                    Data.Item.Image = dr["Image"].ToString();
                    Data.Item.Name = dr["Name"].ToString();
                    Data.Item.Price = Convert.ToInt32(dr["Price"]);
                    DataList.Add(Data);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
        #endregion

        #region 檢查商品在不在購物車
        public bool CheckInCart(string Cart,int Item_Id)
        {
            CartBuy Data = new CartBuy();
            string sql = $@"SELECT * FROM CartBuy m INNER JOIN Item d ON m.Item_Id = d.Id WHERE Cart_Id = '{Cart}' AND Item_Id = '{Item_Id}'; ";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();

                Data.Cart_Id = dr["Cart_Id"].ToString();
                Data.Item_Id = Convert.ToInt32(dr["Item_Id"]);
                Data.Item.Id = Convert.ToInt32(dr["Id"]);
                Data.Item.Image = dr["Image"].ToString();
                Data.Item.Name = dr["Name"].ToString();
                Data.Item.Price = Convert.ToInt32(dr["Price"]);
            }
            catch (Exception e)
            {
                Data = null;//不在購物車內
            }
            finally
            {
                conn.Close();
            }
            return (Data != null);
        }
        #endregion

        #region 商品加入購物車
        public void AddtoCart(string Cart,int Item_Id)
        {
            string sql = $@"INSERT INTO CartBuy(Cart_Id,Item_Id) VALUES ('{Cart}','{Item_Id}');";
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

        #region 移除購物車內商品
        public void RemoveForCart(string Cart,int Item_Id)
        {
            string sql = $@"DELETE FROM CartBuy WHERE Cart_Id = '{Cart}' AND Item_Id = '{Item_Id}';";
            try
            {
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

        #region 檢查購物車是否保存
        public bool CheckCartSave(string Account,string Cart)
        {
            CartSave Data = new CartSave();
            string sql = $@"SELECT * FROM CartSave m INNER JOIN Members d ON m.Account = d.Account WHERE m.Account = '{Account}' AND Cart_Id = '{Cart}';";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Cart_Id = dr["Cart_Id"].ToString();
                Data.Member.Name = dr["Name"].ToString();
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return (Data != null);
        }
        #endregion

        #region 讀取CartSave
        public string GetCartSave(string Account)
        {
            //依會員的Account讀取CartSave資料表內容
            CartSave Data = new CartSave();
            string sql = $@"SELECT * FROM CartSave m INNER JOIN Members d ON m.Account = d.Account WHERE m.Account = '{Account}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Cart_Id = dr["Cart_Id"].ToString();
                Data.Member.Name = dr["Name"].ToString();
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            if (Data != null)
                return Data.Cart_Id;

            //找不到
            return null;
        }
        #endregion

        #region 保存購物車
        public void SaveCart(string Account,string Cart)
        {
            string sql = $@"INSERT INTO CartSave(Account,Cart_Id) VALUES('{Account}','{Cart}');";
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

        #region 取消保存購物車
        public void SaveCartRemove(string Account)
        {
            string sql = $@"DELETE FROM CartSave WHERE Account = '{Account}';";
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
    }
}