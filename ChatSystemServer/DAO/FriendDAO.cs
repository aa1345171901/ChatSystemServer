
namespace ChatSystemServer.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// 用于处理与好友相关的数据库操作
    /// </summary>
    public class FriendDAO
    {
        /// <summary>
        /// 添加好友，根据对方的添加好友的协议进行判断
        /// </summary>
        /// <param name="id">自己的id</param>
        /// <param name="friendId">需要添加的好友id</param>
        /// <returns>返回添加成功与否，以及错误信息</returns>
        public string AddFriendRequest(MySqlConnection mySqlConnection, int id, int friendId)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select FriendshipPolicyId from userdata where id=@id", mySqlConnection);
                cmd.Parameters.AddWithValue("id", friendId);
                if (!reader.Read())
                {
                    return "该id违规或者不存在";
                }

                int friendShipPolicy = reader.GetInt32("FriendshipPolicyId"); // 对方的好友协议，0能直接添加，1需要验证，2不添加好友
                if (friendShipPolicy == 1)
                {
                    cmd.CommandText = "insert into messages set fromuserid=@fromuserid,touserid=@touserid,messagetype=2,messagestate=0";
                    cmd.Parameters.AddWithValue("fromuserid", id);
                    cmd.Parameters.AddWithValue("touserid", friendId);
                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        return "未知错误";
                    }
                    else
                    {
                        return " ";
                    }
                }
                else if (friendShipPolicy == 0)
                {
                    cmd.CommandText = "insert into friend set hostfriendid=@hostfriendid,accetfriendid=@accetfriendid";
                    cmd.Parameters.AddWithValue("hostfriendid", id);
                    cmd.Parameters.AddWithValue("accetfriendid", friendId);
                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        return "未知错误";
                    }
                    else
                    {
                        return " ";
                    }
                }
                else
                {
                    return "对方设置不能添加为好友";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("AddFriend连接数据库发生错误" + e.Message);
                return "服务器出错";
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
        /// 同意添加好友的操作
        /// </summary>
        /// <returns>返回是否成功</returns>
        public bool AgreeAddFriend(MySqlConnection mySqlConnection, int id, int friendId)
        {
            try
            {
                // 执行添加操作
                MySqlCommand cmd = new MySqlCommand("insert into friend set hostFriendid=@hostFriendId,AccetFriendId=@AccetFriendId", mySqlConnection);
                cmd.Parameters.AddWithValue("hostFriendId", friendId);
                cmd.Parameters.AddWithValue("AccetFriendId", id);
                int result = cmd.ExecuteNonQuery();
                if (result == 0)
                {
                    return false;
                }

                if (!HasAdded(mySqlConnection, id, friendId))
                {
                    // 相互添加,如果他未在我的列表中
                    cmd = new MySqlCommand("insert into friend set hostFriendid=@hostFriendId,AccetFriendId=@AccetFriendId", mySqlConnection);
                    cmd.Parameters.AddWithValue("hostfriendid", id);
                    cmd.Parameters.AddWithValue("accetfriendid", friendId);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("agreeAddFriend连接数据库时出错:" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 查看选择的好友是否已经添加
        /// </summary>
        /// <returns>返回是或否</returns>
        public bool HasAdded(MySqlConnection mySqlConnection, int id, int friendId)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("select count(*) from friend where hostfriendid=@hostfriendid and accetfriendid=@accetfriendid", mySqlConnection);
                cmd.Parameters.AddWithValue("hostfriendid", id);
                cmd.Parameters.AddWithValue("accetfriendid", friendId);
                int result = cmd.ExecuteNonQuery();
                if (result == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("HasAdded连接数据库时出错：" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 随机查找数据库中的用户
        /// </summary>
        /// <returns>返回数据库数据集</returns>
        public DataSet RandomSearch(MySqlConnection mySqlConnection)
        {
            MySqlDataAdapter dataAdapter = null;
            DataSet dataSet = null;
            try
            {
                string sql = "SELECT user.id,nickname,age,sex FROM userdata,user where user.dataid=userdata.id ORDER BY RAND() LIMIT 10";
                dataAdapter = new MySqlDataAdapter(sql, mySqlConnection);
                dataSet = new DataSet();
                dataAdapter.SelectCommand.CommandText = sql;
                dataAdapter.Fill(dataSet, "userdata,user");
                return dataSet;
            }
            catch (Exception e)
            {
                Console.WriteLine("RandomSearch连接数据库时出错" + e.Message);
                return null;
            }
            finally
            {
                if (dataAdapter != null)
                {
                    dataAdapter.Dispose();
                }

                if (dataSet != null)
                {
                    dataSet.Dispose();
                }
            }
        }

        /// <summary>
        /// 通过限制年龄和性别查找
        /// </summary>
        /// <param name="ageOption">客户端对年龄的限制</param>
        /// <param name="sexOption">客户端对性别的限制</param>
        /// <returns>返回数据库数据集</returns>
        public DataSet AdvancedSearch(MySqlConnection mySqlConnection, string ageOption, string sexOption)
        {
            MySqlDataAdapter dataAdapter = null;
            DataSet dataSet = null;
            try
            {
                // 查询语句的前半部分
                string sql = "SELECT user.Id,NickName,Age,Sex FROM Userdata,user";
                string ageCondition = ageOption;
                string sexCondition = sexOption;

                if (ageCondition != " " && sexCondition == " ")
                {
                    sql += string.Format(" WHERE {0} and userdata.id=user.dataid", ageCondition);
                }
                else if (ageCondition == " " && sexCondition != " ")
                {
                    sql += string.Format(" WHERE Sex='{0}' and userdata.id=user.dataid", sexCondition);
                }
                else if (ageCondition != " " && sexCondition != " ")
                {
                    sql += string.Format(" WHERE {0} AND Sex='{1}' and userdata.id=user.dataid", ageCondition, sexCondition);
                }

                dataAdapter = new MySqlDataAdapter(sql, mySqlConnection);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "userdata,user");
                return dataSet;
            }
            catch (Exception e)
            {
                Console.WriteLine("AdvancedSearch连接数据库时出错:" + e.Message);
                return null;
            }
        }

        /// <summary>
        /// 基本查找，通过id或昵称查找
        /// </summary>
        /// <returns>返回数据库数据集</returns>
        public DataSet BasicallySearch(MySqlConnection mySqlConnection, int friendId, string nickName)
        {
            MySqlDataAdapter dataAdapter = null;
            DataSet dataSet = null;
            try
            {
                // sql的前置
                string sql = "SELECT user.Id,NickName,Age,Sex FROM Userdata,user";

                sql += string.Format(" WHERE nickname='{0}' and user.dataid=userdata.id", nickName);
                dataAdapter = new MySqlDataAdapter(sql, mySqlConnection);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "userdata,user");

                sql = "SELECT user.Id,NickName,Age,Sex FROM Userdata,user" + string.Format(" WHERE user.id={0} and user.dataid=userdata.id", friendId);
                dataAdapter.SelectCommand.CommandText = sql;
                dataAdapter.Fill(dataSet, "userdata,user");
                return dataSet;
            }
            catch (Exception e)
            {
                Console.WriteLine("BasicallySearch连接数据库时出错:" + e.Message);
                return null;
            }
            finally
            {
                if (dataAdapter != null)
                {
                    dataAdapter.Dispose();
                }

                if (dataSet != null)
                {
                    dataSet.Dispose();
                }
            }
        }

        /// <summary>
        /// 删除好友的操作请求
        /// </summary>
        /// <returns>返回成功与否</returns>
        public bool DeleteFriend(MySqlConnection mySqlConnection, int id, int friendId)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("delete from friend where hostFriendId=@hostFriendId AND accetFriendId=@accetFriendId", mySqlConnection);
                cmd.Parameters.AddWithValue("hostFriendId", id);
                cmd.Parameters.AddWithValue("accetFriendId", friendId);
                int result = cmd.ExecuteNonQuery();
                if (result == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DeleteFriend连接数据库时出错:" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 通过用户id获取好友列表
        /// </summary>
        /// <returns>返回一组键值对</returns>
        public Dictionary<int, (string, int)> GetFriends(MySqlConnection mySqlConnection, int id)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select accetFriendId,NickName,FaceId from UserData,Friend,user where Friend.hostFriendId=@hostFriendId  AND user.id=friend.accetFriendId AND UserData.id=user.dataid", mySqlConnection);
                cmd.Parameters.AddWithValue("hostFriendId", id);
                reader = cmd.ExecuteReader();
                Dictionary<int, (string, int)> friendsDic = new Dictionary<int, (string, int)>();
                while (reader.Read())
                {
                    friendsDic.Add(reader.GetInt32("accetFriendId"), (reader.GetString("NickName"), reader.GetInt32("FaceId")));
                }

                reader.Close();
                return friendsDic;
            }
            catch (Exception e)
            {
                Console.WriteLine("GetFriends连接数据库时出错:" + e.Message);
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
        /// 获取陌生人的昵称和头像
        /// </summary>
        /// <returns>昵称和头像，用‘，’分割</returns>
        public string UpdateStranger(MySqlConnection mySqlConnection, int strangerId)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select nickname, faceid from userdata,user where user.id=@id and user.dataid=userdata.id");
                cmd.Parameters.AddWithValue("id", strangerId);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string res = reader.GetString("nickname") + ",";
                    res += reader.GetInt32("faceid");
                    return res;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateStranger连接数据库时出错:" + e.Message);
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
