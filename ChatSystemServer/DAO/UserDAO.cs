
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
                    int dataId = reader.GetInt32("dataid");
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
        /// 通过传入的dataId获得UserData表的内容
        /// </summary>
        /// <returns>返回UserData对象</returns>
        public UserData GetUserDataByDataId(MySqlConnection mySqlConnection, int dataid)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from userdata where id=@dataid", mySqlConnection);
                cmd.Parameters.AddWithValue("dataid", dataid);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string nickName = reader.GetString("nickname");
                    string sex = reader.GetString("sex");
                    int age = reader.GetInt32("age");
                    string name = reader.GetString("name");
                    int starId = reader.GetInt32("starid");
                    int bloodTypeId = reader.GetInt32("bloodtypeid");
                    int faceId = reader.GetInt32("faceid");
                    return new UserData(nickName, sex, age, name, starId, bloodTypeId, faceId);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetUserDataByDataId连接数据库时错误" + e.Message);
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

        /// <summary>
        /// 注册后选填信息
        /// </summary>
        /// <returns>返回信息操作成功与否</returns>
        public bool Optional(MySqlConnection mySqlConnection, int dataid, string sex, int age, string name, int starid, int bloodtypeid)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("update userdata set sex=@sex,age=@age,name=@name,starid=@starid,bloodtypeid=@bloodtypeid where id=@dataid");
                cmd.Parameters.AddWithValue("sex", sex);
                cmd.Parameters.AddWithValue("age", age);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("starid", starid);
                cmd.Parameters.AddWithValue("bloodtypeid", bloodtypeid);
                cmd.Parameters.AddWithValue("dataid", dataid);
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Optional连接数据库时出错:" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 通过id修改用户信息
        /// </summary>
        /// <returns>返回更改信息是否成功</returns>
        public bool ModifyById(MySqlConnection mySqlConnection, int dataid, string nickName, string sex, int age, string name, int starid, int bloodtypeid, int faceId)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("update userdata set nickname=@nickname,sex=@sex,age=@age,name=@name,starid=@starid,bloodtypeid=@bloodtypeid where id=@dataid", mySqlConnection);
                cmd.Parameters.AddWithValue("nickname", nickName);
                cmd.Parameters.AddWithValue("sex", sex);
                cmd.Parameters.AddWithValue("age", age);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("starid", starid);
                cmd.Parameters.AddWithValue("bloodtypeid", bloodtypeid);
                cmd.Parameters.AddWithValue("dataid", dataid);
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ModifyById连接数据库时出错：" + e.Message);
                return false;
            }
        }
    }
}
