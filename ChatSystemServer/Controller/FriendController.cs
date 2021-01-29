namespace ChatSystemServer.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using ChatSystemServer.DAO;
    using ChatSystemServer.Helper;
    using ChatSystemServer.Servers;
    using Common;

    /// <summary>
    /// 管理与好友相关的各种操作
    /// </summary>
    public class FriendController : BaseController
    {
        private FriendDAO friendDAO;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendController"/> class.
        /// 设置BaseController的reqeuseCode为Friend
        /// </summary>
        public FriendController()
        {
            requestCode = RequestCode.Friend;
            friendDAO = new FriendDAO();
        }

        /// <summary>
        /// 添加好友的操作
        /// </summary>
        /// <returns>返回执行结果</returns>
        public string AddFriend(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int id = int.Parse(strs[0]);
            int friendId = int.Parse(strs[1]);
            if (friendDAO.HasAdded(client.MySqlConnection, id, friendId))
            {
                return ((int)ReturnCode.Fail).ToString() + "已添加对方为好友";
            }
            else
            {
                string result = friendDAO.AddFriendRequest(client.MySqlConnection, id, friendId);
                if (result == " ")
                {
                    return ((int)ReturnCode.Success).ToString();
                }
                else
                {
                    return ((int)ReturnCode.Fail).ToString() + result;
                }
            }
        }

        /// <summary>
        /// 搜索好友请求
        /// 客户端传送的数据为 id,nickName,ageOption,sexOption为空就传输空格或-1
        /// </summary>
        /// <returns>返回数据库数据集</returns>
        public string SearchFriend(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int id = int.Parse(strs[0]);
            string nickName = strs[1];
            string ageOption = strs[2];
            string sexOption = strs[3];
            DataSet dataSet = null;
            if (id != -1 || nickName != " ")
            {
                dataSet = friendDAO.BasicallySearch(client.MySqlConnection, id, nickName);
            }
            else if (ageOption != " " || sexOption != " ")
            {
                dataSet = friendDAO.AdvancedSearch(client.MySqlConnection, ageOption, sexOption);
            }
            else
            {
                dataSet = friendDAO.RandomSearch(client.MySqlConnection);
            }

            if (dataSet != null)
            {
                return ((int)ReturnCode.Success).ToString() + BitConverter.ToString(DataHelper.GetBinaryFormatDataSet(dataSet));
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        /// <summary>
        /// 删除好友的请求
        /// </summary>
        /// <returns>返回操作是否成功以及原因</returns>
        public string DeleteFriend(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int id = int.Parse(strs[0]);
            int friendId = int.Parse(strs[1]);
            if (friendDAO.DeleteFriend(client.MySqlConnection, id, friendId))
            {
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns>返回传送给客户端的信息</returns>
        public string GetFriendList(string data, Client client, Server server)
        {
            Dictionary<int, (string, int)> friends = null;
            friends = friendDAO.GetFriends(client.MySqlConnection, int.Parse(data));
            if (friends != null)
            {
                return ((int)ReturnCode.Success).ToString() + DataHelper.DicToString(friends);
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
    }
}
