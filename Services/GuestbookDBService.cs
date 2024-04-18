using MemberSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace MemberSystem.Services
{
    public class GuestbookDBService
    {
        //DB connect 
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["MemberDBEntities"].ConnectionString;
        //active connect
        private readonly SqlConnection conn = new SqlConnection(cnstr);

        //取得陣列資料方法
        public List<Guestbook> GetDataList(ForPaging Paging, string Search)
        {
            List<Guestbook> DataList = new List<Guestbook>();
            //get table
            //string sql = @"SELECT * FROM Guestbooks;";

            if (!string.IsNullOrWhiteSpace(Search))
            {
                SetMaxPaging(Paging, Search);
                DataList = GetAllDataList(Paging, Search);
            }
            else
            {
                SetMaxPaging(Paging);
                DataList = GetAllDataList(Paging);
            }
            return DataList;
        }

        #region 新增資料
        //define insert record method
        public void InsertGuestbook(Guestbook newData)
        {
            //set time to now
            string sql = $@"INSERT INTO Guestbooks(Name,Content,CreateTime) VALUES ('{newData.Name}','{newData.Content}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');";

            try
            {
                //start db connect
                conn.Open();
                //do insert instruction
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //show exception
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                //close db connect
                conn.Close();
            }
        }
        #endregion

        #region 查詢一筆資料
        //search by Id
        public Guestbook GetDataById(int Id)
        {
            Guestbook Data = new Guestbook();
            string sql = $@"SELECT * FROM Guestbooks WHERE Id = {Id};";
            try
            {
            //start db connect
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                //get query result
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Id = Convert.ToInt32(dr["Id"]);
                Data.Name = dr["Name"].ToString();
                Data.Content = dr["Content"].ToString();
                Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                //check reply except whitespace
                if(!string.IsNullOrWhiteSpace(dr["Reply"].ToString()))
                {
                    Data.Reply = dr["Reply"].ToString();
                    Data.ReplyTime = Convert.ToDateTime(dr["ReplyTime"]);
                }
            }
            catch (Exception e)
            {
                //not found
                Data = null;
            }
            finally
            {
                //end db connect
                conn.Close();
            }
            return Data;
        }
        #endregion

        #region 修改留言
        //define method
        public void UpdateGuestbook(Guestbook UpdateData)
        {
            string sql = $@"UPDATE Guestbooks SET Name = '{UpdateData.Name}',Content = '{UpdateData.Content}' WHERE Id = {UpdateData.Id};";

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

        #region 回覆留言
        //define method
        public void ReplyGuestbook(Guestbook ReplyData)
        {
            string sql = $@"UPDATE Guestbooks SET Reply = '{ReplyData.Reply}',ReplyTime = '{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}' WHERE Id = {ReplyData.Id};";
            try
            {
                //start db connect
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

        #region 動作前檢查
        public bool CheckUpdate(int Id)
        {
            Guestbook Data = GetDataById(Id);
            return (Data != null && Data.ReplyTime == null);
        }
        #endregion

        #region 刪除留言
        public void DeleteGuestbook(int Id)
        {
            string sql = $@"DELETE FROM Guestbooks WHERE Id = {Id};";
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

        #region 關鍵字查詢
        public List<Guestbook> GetDataList(string Search)
        {
            List<Guestbook> DataList = new List<Guestbook>();
            string sql = string.Empty;

            if (!string.IsNullOrWhiteSpace(Search))
            {
                //name、content、reply have search string
                sql = $@"SELECT * FROM Guestbooks WHERE Name LIKE '%{Search}%' OR Content LIKE '%{Search}%' OR Reply LIKE '{Search}';";
            }
            else
            {
                //show all
                sql = $@"SELECT * FROM Guestbooks;";
            }
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Guestbook Data = new Guestbook();
                    Data.Id = Convert.ToInt32(dr["Id"]);
                    Data.Name = dr["Name"].ToString();
                    Data.Content = dr["Content"].ToString();
                    Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    //reply check
                    if (!dr["ReplyTime"].Equals(DBNull.Value))
                    {
                        Data.Reply = dr["Reply"].ToString();
                        Data.ReplyTime = Convert.ToDateTime(dr["ReplyTime"]);
                    }
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

        #region 設定最大頁數
        public void SetMaxPaging(ForPaging Paging)
        {
            int Row = 0;
            string sql = $@"SELECT * FROM Guestbooks;";

            try
            {
                //start connect
                conn.Open();
                //do sql command
                SqlCommand cmd = new SqlCommand(sql, conn);
                //get sql data
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read()) //do till no data
                {
                    Row++;
                }
            }
            catch (Exception e)
            {
                //exception handle
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                //end connect
                conn.Close();
            }
            //count page
            Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Row) / Paging.ItemNum));
            Paging.SetRightPage();
        }

        public void SetMaxPaging(ForPaging Paging,string Search)
        {
            int Row = 0;
            string sql = $@"SELECT * FROM Guestbooks WHERE Name LIKE '%{Search}%' OR Content LIKE '%{Search}%' OR Reply LIKE '%{Search}%';";
            try
            {
                conn.Open();
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

        #region 搜尋資料
        public List<Guestbook> GetAllDataList(ForPaging paging)
        {
            List<Guestbook> DataList = new List<Guestbook>();
            string sql = $@"SELECT * FROM (SELECT row_number() OVER(ORDER BY Id) AS sort,* FROM Guestbooks) m WHERE m.sort BETWEEN {(paging.NowPage-1)*paging.ItemNum+1} AND {paging.NowPage*paging.ItemNum};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                /*               while (dr.Read())
                               {
                                   Guestbook Data = new Guestbook();
                                   Data.Id = Convert.ToInt32(dr["Id"]);
                                   Data.Name = dr["Name"].ToString();
                                   Data.Content = dr["Content"].ToString();
                                   Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                                   //adding in if have replytime
                                   if (!dr["ReplyTime"].Equals(DBNull.Value))
                                   {
                                       Data.Reply = dr["Reply"].ToString();
                                       Data.ReplyTime = Convert.ToDateTime(dr["ReplyTime"]);
                                   }
                                   DataList.Add(Data);
                               }*/
                GDL(dr, DataList);
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

        public List<Guestbook> GetAllDataList(ForPaging paging,string Search)
        {
            List<Guestbook> DataList = new List<Guestbook>();
            string sql = $@"SELECT * FROM (SELECT row_number() OVER(order by Id) AS sort,* FROM Guestbooks WHERE Name LIKE '%{Search}%' OR Content LIKE '%{Search}%' OR Reply LIKE '%{Search}%') m WHERE m.sort BETWEEN {(paging.NowPage - 1) * paging.ItemNum + 1} AND {paging.NowPage * paging.ItemNum};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                GDL(dr, DataList);
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

        #region 產生DataList
        public void GDL(SqlDataReader vdr,List<Guestbook> vDL)
        {
            while (vdr.Read())
            {
                Guestbook Data = new Guestbook();
                Data.Id = Convert.ToInt32(vdr["Id"]);
                Data.Name = vdr["Name"].ToString();
                Data.Content = vdr["Content"].ToString();
                Data.CreateTime = Convert.ToDateTime(vdr["CreateTime"]);
                //adding in if have replytime
                if (!vdr["ReplyTime"].Equals(DBNull.Value))
                {
                    Data.Reply = vdr["Reply"].ToString();
                    Data.ReplyTime = Convert.ToDateTime(vdr["ReplyTime"]);
                }
                vDL.Add(Data);
            }
        }
        #endregion
    }
}