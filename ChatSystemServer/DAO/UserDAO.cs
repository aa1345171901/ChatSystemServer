
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
    }
}
