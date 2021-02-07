
namespace ChatSystemServer.DAO
{
    using System;
    using ChatSystemServer.Model;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// 用于处理数据库中User表的各项需求
    /// </summary>
    public class UserDAO
    {
        /// <summary>
        /// 用于验证账户是否存在
        /// </summary>
        /// <returns>返回user对象供Controller使用</returns>
        public User VerifyUser(MySqlConnection mysqlConnection, int id, string password)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where id=@id and password=@password", mysqlConnection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("password", password);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int dataId = (int)reader["dataid"];
                    return new User(id, password, dataId);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("VerifyUser连接数据库时出错:" + e.Message);
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
        /// 通过昵称创建账号
        /// </summary>
        /// <returns>返回user，其中id是自动增长</returns>
        public User AddUser(MySqlConnection mysqlConnection, string nickName, string password)
        {
            MySqlDataReader reader = null;
            try
            {
                int dataId;
                MySqlCommand cmd = new MySqlCommand("insert into userdata set nickname=@nickName", mysqlConnection);
                cmd.Parameters.AddWithValue("nickName", nickName);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dataId = reader.GetInt32("id");
                    cmd.CommandText = "insert into user set password=@password, dataid=@dataid";
                    cmd.Parameters.AddWithValue("password", password);
                    cmd.Parameters.AddWithValue("dataid", dataId);
                    int result = cmd.ExecuteNonQuery();
                    if (result == 1)
                    {
                        int id = (int)cmd.LastInsertedId;
                        User user = new User(id, password, dataId);
                        return user;
                    }
                    else
                    {
                        Console.WriteLine("插入用户时");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("插入用户数据时");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("AddUser连接数据库时出错:" + e.Message);
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
