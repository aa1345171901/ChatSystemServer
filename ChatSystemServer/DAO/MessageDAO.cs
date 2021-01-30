
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ChatSystemServer.DAO
{
    /// <summary>
    /// 管理数据库messages表
    /// </summary>
    public class MessageDAO
    {
        /// <summary>
        /// 根据用户id获取未读的消息的message信息
        /// </summary>
        /// <returns>返回数据</returns>
        public List<string> GetUnreadMessage(MySqlConnection mySqlConnection, int id)
        {
            MySqlDataReader reader = null;
            try
            {
                List<string> list = new List<string>();
                int fromUserId = 0, messageTypeId = 0, messageState = 0, havePlayAdiuo = 0;
                MySqlCommand cmd = new MySqlCommand("select  FromUserId, MessageType, MessageState,havePlayAdiuo,msgId from Messages where touserid=@id and messagestate=0", mySqlConnection);
                cmd.Parameters.AddWithValue("id", id);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    fromUserId = reader.GetInt32("FromUserId");
                    messageTypeId = reader.GetInt32("MessageType");
                    messageState = reader.GetInt32("MessageState");
                    havePlayAdiuo = reader.GetInt32("havePlayAdiuo");
                    string s = fromUserId + "," + messageTypeId + "," + messageState + "," + havePlayAdiuo;
                    if (messageTypeId == 1 && messageState == 0)
                    {
                        cmd.CommandText = "SELECT FaceId FROM Userdata,user WHERE userdata.id=user.dataid and user.id=@userid";
                        cmd.Parameters.AddWithValue("userid", id);
                        int friendFaceId = Convert.ToInt32(cmd.ExecuteScalar());   // 设置发消息的好友的头像索引
                        s += "," + friendFaceId;
                    }
                    else
                    {
                        s += ", ";
                    }

                    list.Add(s);
                    cmd.CommandText = "update messages set haveplayadiuo=1 where touserid=@id";
                    cmd.ExecuteNonQuery();
                }

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine("GetUnreadMessage访问数据库时出错:" + e.Message);
                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
