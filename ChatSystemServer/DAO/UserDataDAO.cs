
namespace ChatSystemServer.DAO
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;
    using ChatSystemServer.Model;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// 用于操作UserData表的操作
    /// </summary>
    public class UserDataDAO
    {
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
                    string nickName = reader["nickname"] as string;
                    string sex = reader["sex"] as string;
                    int age = (int)reader["age"];
                    string name = reader["name"] as string;
                    int starId = 0;
                    int.TryParse(reader["starid"].ToString(), out starId);
                    int bloodTypeId = 0;
                    int.TryParse(reader["bloodtypeid"].ToString(), out bloodTypeId);
                    int faceId = (int)reader["faceid"];
                    return new UserData(dataid, nickName, sex, age, name, starId, bloodTypeId, faceId);
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
        /// 注册后选填信息
        /// </summary>
        /// <returns>返回信息操作成功与否</returns>
        public bool Optional(MySqlConnection mySqlConnection, int dataid, string sex, int age, string name, int starid, int bloodtypeid)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("update userdata set sex=@sex,age=@age,name=@name,starid=@starid,bloodtypeid=@bloodtypeid where id=@dataid", mySqlConnection);
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
        public bool ModifyById(MySqlConnection mySqlConnection, int dataid, string nickName, string sex, int age, string name, int starid, int bloodtypeid)
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

        /// <summary>
        /// 设置头像，使用系统内置头像
        /// </summary>
        /// <returns>返回修改是否成功</returns>
        public bool SetSystemFace(MySqlConnection mySqlConnection, int dataId, int faceId)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("update userdata set faceid=@faceId where id=@dataid", mySqlConnection);
                cmd.Parameters.AddWithValue("faceid", faceId);
                cmd.Parameters.AddWithValue("dataid", dataId);
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
                Console.WriteLine("SetSystemFace访问数据库时出错:" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 将传送过来的头像保存，并且设置该角色的faceid
        /// </summary>
        /// <returns>返回设置是否成功</returns>
        public bool SetSelfFace(MySqlConnection mySqlConnection, int dataid, string data)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into face set path=@path", mySqlConnection);
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\Face\";
                cmd.Parameters.AddWithValue("path", path);
                int faceid = 0;
                faceid = (int)cmd.ExecuteScalar();
                if (faceid != 1)
                {
                    return false;
                }

                cmd.CommandText = "update userdata set faceid=@faceid where id=@dataid";
                cmd.Parameters.AddWithValue("faceid", faceid);
                cmd.Parameters.AddWithValue("dataid", dataid);
                int result = cmd.ExecuteNonQuery();
                if (result != 1)
                {
                    return false;
                }

                MemoryStream fs = new MemoryStream();
                byte[] dataBytesImg = Encoding.UTF8.GetBytes(data);
                int len = dataBytesImg.Length;
                fs.Write(dataBytesImg, 0, len);
                Bitmap img = new Bitmap(fs);
                string filename = faceid + ".jpg";
                img.Save(path + filename, ImageFormat.Jpeg);
                cmd.CommandText = "update face set path=@imgpath where id=@faceid";
                cmd.Parameters.AddWithValue("imagpath", path + filename);
                cmd.Parameters.AddWithValue("faceid", faceid);
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("SetSelfFace连接数据库时出错:" + e.Message);
                return false;
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
