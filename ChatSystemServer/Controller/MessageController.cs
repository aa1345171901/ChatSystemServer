namespace ChatSystemServer.Controller
{
    using ChatSystemServer.DAO;
    using ChatSystemServer.Servers;
    using Common;
    using System.Collections.Generic;

    /// <summary>
    /// 管理聊天信息和系统信息的各种操作
    /// </summary>
    public class MessageController : BaseController
    {

        private MessageDAO messageDAO;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageController"/> class.
        /// </summary>
        public MessageController()
        {
            this.requestCode = RequestCode.Message;
            messageDAO = new MessageDAO();
        }

        /// <summary>
        /// 获取未读的消息
        /// </summary>
        /// <returns>返回消息的属性</returns>
        public string GetUnreadMessage(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            List<string> list = null;
            list = messageDAO.GetUnreadMessage(client.MySqlConnection, int.Parse(data));
            if (list != null)
            {
                string resData = "";
                foreach (var item in list)
                {
                    resData += item + "-"; // 使用“-”将不同信息分割
                }

                return ((int)ReturnCode.Success).ToString() + "," + resData;
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
    }
}
