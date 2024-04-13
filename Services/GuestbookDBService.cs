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
        public List<Guestbook> GetDataList()
        {
            List<Guestbook> DataList = new List<Guestbook>();
            //get table
            string sql = @"SELECT * FROM Guestbooks;";
            try
            {
                //start connect
                conn.Open();
                //do sql command
                SqlCommand cmd = new SqlCommand(sql,conn);
                //get sql data
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read()) //do till no data
                {
                    Guestbook Data = new Guestbook();
                    Data.Id = Convert.ToInt32(dr["Id"]);
                    Data.Name = dr["Name"].ToString();
                    Data.Content = dr["Content"].ToString();
                    Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    //check wether have reply
                    //null can't convert to datetime
                    if(!dr["ReplyTime"].Equals(DBNull.Value))
                    {
                        Data.Reply = dr["Reply"].ToString();
                        Data.ReplyTime = Convert.ToDateTime(dr["ReplyTime"]);
                    }
                    DataList.Add(Data);
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
    }
}