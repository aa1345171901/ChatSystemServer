namespace ChatSystemServer.Controller
{
    using System;
    using System.Collections.Generic;
    using ChatSystemServer.DAO;
    using ChatSystemServer.Helper;
    using ChatSystemServer.Model;
    using ChatSystemServer.Servers;
    using Common;

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
            List<string> list = null;
            list = messageDAO.GetUnreadMessage(client.MySqlConnection, int.Parse(data));
            if (list != null && list.Count != 0)
            {
                string resData = "";
                foreach (var item in list)
                {
                    resData += item; // 使用“-”将不同信息分割
                }

                return ((int)ReturnCode.Success).ToString() + "," + resData;
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        /// <summary>
        /// 对好友请求进行响应，将消息标为已读
        /// </summary>
        /// <returns>返回获取的信息</returns>
        public string AddFriendMessageRequest(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int id = int.Parse(strs[0]);
            int fromUserId = int.Parse(strs[1]);
            string result = "";
            result = messageDAO.SetMessageRead(client.MySqlConnection, id, fromUserId);
            if (result != "")
            {
                return ((int)ReturnCode.Success).ToString() + "," + fromUserId + "," + result;
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString() + "," + fromUserId;
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
            DateTime sendTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            sendTime = sendTime.AddMilliseconds(long.Parse(strs[3]));
            Messages msg = new Messages(fromUserId, toUserId, message, sendTime);
            if (messageDAO.SendToChat(client.MySqlConnection, msg))
            {
                return ((int)ReturnCode.Success).ToString() + "," + toUserId;
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString() + "," + toUserId;
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <returns>返回消息内容和发送时间</returns>
        public string ChatByReceive(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int id = int.Parse(strs[0]);
            int friendId = int.Parse(strs[1]);
            Messages message = null;
            message = messageDAO.ReceiveToChat(client.MySqlConnection, id, friendId);
            if (message != null)
            {
                return ((int)ReturnCode.Success).ToString() + "," + friendId + "," + message.ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString() + "," + friendId;
            }
        }
    }
}
