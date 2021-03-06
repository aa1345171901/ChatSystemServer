namespace ChatSystemServer.Helper
{
    using System;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// 用于处理数据库
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// 连接数据库的字符串
        /// </summary>
        public const string CONNECTIONSTRING = "database=chat_system;data source=127.0.0.1;port=3306;user=root;password=root;";

        /// <summary>
        /// 返回一个MySqlConnection给服务器用于操作数据库
        /// </summary>
        /// <returns>返回一个MySqlConnection对象</returns>
        public static MySqlConnection Connect()
        {
            MySqlConnection mySqlConnection = new MySqlConnection(CONNECTIONSTRING);
            try
            {
                mySqlConnection.Open();
                return mySqlConnection;
            }
            catch (Exception e)
            {
                Console.WriteLine("连接数据库时出现异常" + e.Message);
                return null;
            }
        }

        /// <summary>
        /// 用于关闭数据库连接
        /// </summary>
        /// <param name="mySqlConnection">传入需要关闭的数据库对象</param>
        public static void Close(MySqlConnection mySqlConnection)
        {
            if (mySqlConnection != null)
            {
                mySqlConnection.Close();
            }
            else
            {
                Console.WriteLine("MySqlConnection不能为空");
            }
        }
    }
}
