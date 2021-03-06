﻿
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
        public Dictionary<int, string> GetUnreadMessage(MySqlConnection mySqlConnection, int id)
        {
            MySqlDataReader reader = null;
            try
            {
                Dictionary<int, string> dict1 = new Dictionary<int, string>();
                Dictionary<int, string> dict2 = new Dictionary<int, string>();
                int msgid = 0, fromUserId = 0, messageTypeId = 0, messageState = 0, havePlayAdiuo = 0;
                MySqlCommand cmd = new MySqlCommand("select  msgid, FromUserId, MessageType, MessageState,havePlayAdiuo,msgId from Messages where touserid=@id and messagestate=0", mySqlConnection);
                cmd.Parameters.AddWithValue("id", id);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    msgid = (int)reader["msgid"];
                    fromUserId = (int)reader["FromUserId"];
                    messageTypeId = (int)reader["MessageType"];
                    messageState = (int)reader["MessageState"];
                    havePlayAdiuo = (int)reader["havePlayAdiuo"];
                    string s = fromUserId + "," + messageTypeId + "," + messageState + "," + havePlayAdiuo;
                    dict1.Add(msgid, s);
                }

                reader.Close();

                foreach (var item in dict1)
                {
                    // if (messageTypeId == 1 && messageState == 0)
                    // {
                    MySqlCommand cmdGet = new MySqlCommand("SELECT FaceId FROM Userdata,user WHERE userdata.id=user.dataid and user.id=@userid", mySqlConnection);
                    cmdGet.Parameters.AddWithValue("userid", int.Parse(item.Value.Split(',')[0]));
                    int friendFaceId = Convert.ToInt32(cmdGet.ExecuteScalar());   // 设置发消息的好友的头像索引

                    Messages msg = new Messages();
                    int msgId = 0;
                    MySqlCommand cmdMsg = new MySqlCommand("select msgid,message,sendtime from messages where fromuserid=@fromuserid and touserid=@touserid and messagetype=1 and messagestate=0 order by sendtime desc limit 1", mySqlConnection);
                    cmdMsg.Parameters.AddWithValue("fromuserid", int.Parse(item.Value.Split(',')[0]));
                    cmdMsg.Parameters.AddWithValue("touserid", id);
                    reader = cmdMsg.ExecuteReader();
                    if (reader.Read())
                    {
                        msg.Message = (string)reader["message"];
                        msg.SendTime = Convert.ToDateTime(reader["sendtime"]);
                        msgId = (int)reader["msgid"];
                    }

                    reader.Close();

                    string sql = "select nickname from userdata,user where user.dataid=userdata.id and user.id=@fromUserId";
                    cmdMsg.CommandText = sql;
                    reader = cmdMsg.ExecuteReader();

                    string nickName = "";
                    if (reader.Read())
                    {
                        nickName = reader["nickName"].ToString();
                    }

                    string s = item.Value + "," + friendFaceId + "," + nickName + "," + msg.ToString();
                    dict2.Add(item.Key, s);
                    reader.Close();
                    // }
                    // else
                    // {
                    //     s += ", ";
                    // }
                }

                cmd.CommandText = "update messages set haveplayadiuo=1 where touserid=@id";
                cmd.ExecuteNonQuery();

                return dict2;
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
                    int msgId = (int)reader["msgid"];
                    int fromUserId = (int)reader["fromuserid"];

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
                    int faceId = (int)dataReader["faceid"];
                    string nickName = (string)dataReader["nickname"];
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
        public (Messages, string) ReceiveToChat(MySqlConnection mySqlConnection, int id, int friendId)
        {
            MySqlDataReader reader = null;
            try
            {
                Messages msg = new Messages();
                int msgId = 0;
                MySqlCommand cmd = new MySqlCommand("select msgid,message,sendtime from messages where fromuserid=@fromuserid and touserid=@touserid and messagetype=1 and messagestate=0 order by sendtime desc limit 1", mySqlConnection);
                cmd.Parameters.AddWithValue("fromuserid", friendId);
                cmd.Parameters.AddWithValue("touserid", id);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    msg.Message = (string)reader["message"];
                    msg.SendTime = Convert.ToDateTime(reader["sendtime"]);
                    msgId = (int)reader["msgid"];
                }
                else
                {
                    return (null, null);
                }

                reader.Close();
                cmd.CommandText = "update messages set messagestate=1 where msgid=@msgId";
                cmd.Parameters.AddWithValue("msgId", msgId);
                cmd.ExecuteNonQuery();

                string sql = "select nickname from userdata,user where user.dataid=userdata.id and user.id=@fromUserId";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();

                string nickName = "";
                if (reader.Read())
                {
                    nickName = reader["nickName"].ToString();
                }

                return (msg, nickName);
            }
            catch (Exception e)
            {
                Console.WriteLine("ReceiveToChat连接数据库时出错:" + e.Message);
                return (null, null);
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
                List<int> msgsId = new List<int>();

                // 查找一个未读消息
                MySqlCommand cmd = new MySqlCommand("select msgid from messages where fromuserid=@fromUserId and touserid=@loginid and messagetype=2 and messagestate=0", mySqlConnection);
                cmd.Parameters.AddWithValue("loginid", id);
                cmd.Parameters.AddWithValue("fromUserId", fromUserId);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    msgsId.Add((int)reader["msgid"]);
                }

                reader.Close();

                // 将消息状态置为已读
                foreach (var msgId in msgsId)
                {
                    MySqlCommand cmdMsg = new MySqlCommand("update messages set messagestate=1 where msgid=@msgId", mySqlConnection);
                    cmdMsg.Parameters.AddWithValue("msgId", msgId);
                    cmdMsg.ExecuteNonQuery();
                }

                string sql = "select nickname from userdata,user where user.dataid=userdata.id and user.id=@fromUserId";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();

                string nickName = "";
                if (reader.Read())
                {
                    nickName = reader["nickName"].ToString();
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

        /// <summary>
        /// 接收目标好友所有未读消息
        /// </summary>
        /// <returns>返回获取的消息</returns>
        public List<Messages> GetReceiveMsgs(MySqlConnection mySqlConnection, int id, int friendId)
        {
            MySqlDataReader reader = null;
            try
            {
                List<Messages> msgs = new List<Messages>();
                List<int> msgsId = new List<int>();
                MySqlCommand cmd = new MySqlCommand("select msgid,message,sendtime from messages where fromuserid=@fromuserid and touserid=@touserid and messagetype=1 and messagestate=0 order by sendtime desc", mySqlConnection);
                cmd.Parameters.AddWithValue("fromuserid", friendId);
                cmd.Parameters.AddWithValue("touserid", id);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Messages msg = new Messages();
                    msg.Message = (string)reader["message"];
                    msg.SendTime = Convert.ToDateTime(reader["sendtime"]);
                    msgsId.Add((int)reader["msgid"]);
                    msgs.Add(msg);
                }

                reader.Close();

                foreach (var msgId in msgsId)
                {
                    MySqlCommand cmdMsg = new MySqlCommand("update messages set messagestate=1 where msgid=@msgId", mySqlConnection);
                    cmdMsg.Parameters.AddWithValue("msgId", msgId);
                    cmdMsg.ExecuteNonQuery();
                }

                return msgs.Count == 0 ? null : msgs;
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
    }
}
