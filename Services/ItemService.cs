using MemberSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MemberSystem.Services
{
    public class ItemService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["MemberDB"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);

        #region 讀取個別商品資料
        public Item GetDataById(int Id)
        {
            Item Data = new Item();
            string sql = $@"SELECT * FROM Item WHERE Id = {Id}";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Id = Convert.ToInt32(dr["Id"]);
                Data.Image = dr["Image"].ToString();
                Data.Name = dr["Name"].ToString();
                Data.Price = Convert.ToInt32(dr["Price"]);
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        #endregion

        #region 商品編號陣列
        public List<int> GetIdList(ForPaging Paging)
        {
            //calculate needed page amount
            SetMaxPaging(Paging);
            //get item datalist from db
            List<int> IdList = new List<int>();
            string sql = $@"SELECT * FROM (SELECT row_number() OVER(order by Id desc) AS sort,* FROM Item) m WHERE m.sort BETWEEN 
                {(Paging.NowPage-1)*Paging.ItemNum+1} AND {Paging.NowPage*Paging.ItemNum};";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    IdList.Add(Convert.ToInt32(dr["Id"]));
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
            return IdList;
        }
        #endregion

        #region 設定最大頁數
        public void SetMaxPaging(ForPaging Paging)
        {
            int Row = 0;
            string sql = $@"SELECT * FROM Item;";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Row++;
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
            Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Row) / Paging.ItemNum));
            Paging.SetRightPage();
        }
        #endregion

        #region 新增商品
        public void Insert(Item newData)
        {
            string sql = $@"INSERT INTO Item(Name,Price,Image) VALUES ('{newData.Name}','{newData.Price}','{newData.Image}')";
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

        #region 刪除商品
        public void Delete(int Id)
        {
            //依商品號刪資料
            //先刪CartBuy才Item
            //use StringBuilder to expend string length limit
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($@"DELETE FROM CartBuy WHERE Item_Id = {Id};");
            sql.AppendLine($@"DELETE FROM Item WHERE Id = {Id};");
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql.ToString(), conn);
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