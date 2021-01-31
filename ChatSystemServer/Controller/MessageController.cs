namespace ChatSystemServer.Controller
{
    using ChatSystemServer.DAO;
    using ChatSystemServer.Helper;
    using ChatSystemServer.Model;
    using ChatSystemServer.Servers;
    using Common;
    using System;
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
        /// 获取未读的消息，定期获取,给客户端一个是否有未读消息的信息
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

        /// <summary>
        /// 对好友请求进行响应，获取好友的id和昵称
        /// </summary>
        /// <returns>返回获取的信息</returns>
        public string AddFriendMessageRequest(string data, Client client, Server server)
        {
            Dictionary<int, (string, int)> messageDics = new Dictionary<int, (string, int)>();
            if (messageDics != null)
            {
                return ((int)ReturnCode.Success).ToString() + DataHelper.DicToString(messageDics);
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        /// <summary>
        /// 给好友发送消息,
        /// </summary>
        /// <param name="data">fromuserId,toUserId,message,sendTime</param>
        /// <returns>返回成功与否</returns>
        public string SendByChat(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int fromUserId = int.Parse(strs[0]);
            int toUserId = int.Parse(strs[1]);
            string message = strs[2];
            DateTime sendTime = new DateTime(long.Parse(strs[3]));
            Messages msg = new Messages(fromUserId, toUserId, message, sendTime);
            if (messageDAO.SendToChat(client.MySqlConnection, msg))
            {
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        public string ChatByReceive(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int id = int.Parse(strs[0]);
            int friendId = int.Parse(strs[1]);
            Messages message = null;
            message = messageDAO.ReceiveToChat(client.MySqlConnection, id, friendId);
            if (message != null)
            {
                return ((int)ReturnCode.Success).ToString() + message.ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
    }
}
