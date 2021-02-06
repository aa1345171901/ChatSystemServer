
namespace ChatSystemServer.DAO
{
    using System;
    using System.Collections.Generic;
    using ChatSystemServer.Model;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// 管理数据库messages表
    /// </summary>
    public class MessageDAO
    {
        /// <summary>
        /// 根据用户id获取未读的消息的message信息,定期获取
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

                    // if (messageTypeId == 1 && messageState == 0)
                    // {
                    cmd.CommandText = "SELECT FaceId FROM Userdata,user WHERE userdata.id=user.dataid and user.id=@userid";
                    cmd.Parameters.AddWithValue("userid", id);
                    int friendFaceId = Convert.ToInt32(cmd.ExecuteScalar());   // 设置发消息的好友的头像索引
                    s += "," + friendFaceId;

                    // }
                    // else
                    // {
                    //     s += ", ";
                    // }
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

        /// <summary>
        /// h获取添加好友的系统消息，并设置消息为已读
        /// </summary>
        /// <returns>返回空或者好友信息</returns>
        public Dictionary<int, (string, int)> GetFriendDataByMessage(MySqlConnection mySqlConnection, int id)
        {
            MySqlDataReader reader = null;
            MySqlDataReader dataReader = null;
            try
            {
                Dictionary<int, (string, int)> msgId_idDicts = new Dictionary<int, (string, int)>();
                MySqlCommand cmd = new MySqlCommand("select msgid,FromUserId from messages where touserid=@id and messagetype=2 and messagestate=0", mySqlConnection);
                cmd.Parameters.AddWithValue("id", id);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int msgId = reader.GetInt32("msgid");
                    int fromUserId = reader.GetInt32("fromuserid");

                    // 将消息状态置为已读
                    string sql = "UPDATE Messages SET MessageState =1 WHERE msgid=@messageid";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("messageid", msgId);
                    cmd.ExecuteNonQuery();

                    // 读取请求人的信息，显示在窗体上
                    sql = "SELECT NickName, FaceId FROM Userdata,user WHERE user.Id=@fromuserid and user.dataid=userdata.id";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("fromuserid", fromUserId);
                    dataReader = cmd.ExecuteReader();
                    int faceId = dataReader.GetInt32("faceid");
                    string nickName = dataReader.GetString("nickname");
                    dataReader.Close();
                    msgId_idDicts.Add(fromUserId, (nickName, faceId));
                }

                return msgId_idDicts;
            }
            catch (Exception e)
            {
                Console.WriteLine("GetFriendDataByMessage访问数据库时出错:" + e.Message);
                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
        }

        /// <summary>
        /// 给好友发送消息
        /// </summary>
        /// <returns>返回是否发送成功</returns>
        public bool SendToChat(MySqlConnection mySqlConnection, Messages message)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into messages  set fromuserid=@fromuserid,touserid=@touserid,message=@message,sendtime=@sendtime,messagetype=1,messagestate=0", mySqlConnection);
                cmd.Parameters.AddWithValue("fromuserid", message.FromUserId);
                cmd.Parameters.AddWithValue("touserid", message.ToUserId);
                cmd.Parameters.AddWithValue("message", message.Message);
                cmd.Parameters.AddWithValue("sendtime", message.SendTime);
                int result = cmd.ExecuteNonQuery();
                if (result == 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("SendToChat访问数据库时出错:" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <returns>返回获取的消息</returns>
        public Messages ReceiveToChat(MySqlConnection mySqlConnection, int id, int friendId)
        {
            MySqlDataReader reader = null;
            try
            {
                Messages msg = null;
                int msgId = 0;
                MySqlCommand cmd = new MySqlCommand("select msgid,message,sendtime from messages where fromuserid=@fromuserid and touserid=@touserid and messagetype=1 and messagestate=0 order by sendtime asc limit 1", mySqlConnection);
                cmd.Parameters.AddWithValue("fromuserid", friendId);
                cmd.Parameters.AddWithValue("touserid", id);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    msg.Message = reader.GetString("message");
                    msg.SendTime = Convert.ToDateTime(reader["sendtime"]);
                    msgId = reader.GetInt32("msgid");
                }

                cmd.CommandText = "update messages set messagetstate=1 where msgid=@msgId";
                cmd.Parameters.AddWithValue("msgId", msgId);
                cmd.ExecuteNonQuery();

                return msg;
            }
            catch (Exception e)
            {
                Console.WriteLine("ReceiveToChat连接数据库时出错:" + e.Message);
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

        /// <summary>
        /// 设置目标消息为已读
        /// </summary>
        /// <returns>返回成功与否</returns>
        public string SetMessageRead(MySqlConnection mySqlConnection, int id, int fromUserId)
        {
            MySqlDataReader reader = null;
            try
            {
                int messageId = 0;

                // 查找一个未读消息
                MySqlCommand cmd = new MySqlCommand("select msgid from messages where fromuserid=@fromUserId and touserid=@loginid and messagetype=2 and messagestate=0", mySqlConnection);
                cmd.Parameters.AddWithValue("loginid", id);
                cmd.Parameters.AddWithValue("fromUserId", fromUserId);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    messageId = (int)reader["msgid"];
                }

                // 将消息状态置为已读
                string sql = "UPDATE Messages SET MessageState =1 WHERE msgid=@messageid";
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("messageid", messageId);
                cmd.ExecuteNonQuery();

                sql = "select nickname from userdata,user where user.dataid=userdata.id and user.id=@fromuserid";
                cmd.Parameters.AddWithValue("fromuserid", fromUserId);
                reader = cmd.ExecuteReader();

                string nickName = "";
                if (reader.Read())
                {
                    nickName = reader["nickName"] as string;
                }

                return nickName;
            }
            catch (Exception e)
            {
                Console.WriteLine("SetMessageRead访问数据库时出错:" + e.Message);
                return "";
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
