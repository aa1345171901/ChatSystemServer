﻿namespace ChatSystemServer.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using ChatSystemServer.DAO;
    using ChatSystemServer.Helper;
    using ChatSystemServer.Model;
    using ChatSystemServer.Servers;
    using Common;

    /// <summary>
    /// 管理与好友相关的各种操作
    /// </summary>
    public class FriendController : BaseController
    {
        private FriendDAO friendDAO;
        private UserDAO userDAO;
        private UserDataDAO userDataDAO;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendController"/> class.
        /// 设置BaseController的reqeuseCode为Friend
        /// </summary>
        public FriendController()
        {
            requestCode = RequestCode.Friend;
            friendDAO = new FriendDAO();
            userDAO = new UserDAO();
            userDataDAO = new UserDataDAO();
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
            if (id == friendId)
            {
                return ((int)ReturnCode.Fail).ToString() + "," + "不能添加自己";
            }

            if (friendDAO.HasAdded(client.MySqlConnection, id, friendId))
            {
                return ((int)ReturnCode.Fail).ToString() + "," + "已添加对方为好友";
            }
            else
            {
                string result = friendDAO.AddFriendRequest(client.MySqlConnection, id, friendId);
                if (result == " ")
                {
                    // 给好友发送一份
                    if (server.GetChatReceive(friendId) != null)
                    {
                        server.RequestHander(RequestCode.Message, ActionCode.GetUnreadMessage, friendId.ToString(), server.GetChatReceive(friendId));
                    }

                    return ((int)ReturnCode.Success).ToString();
                }
                else
                {
                    return ((int)ReturnCode.Fail).ToString() + "," + result;
                }
            }
        }

        /// <summary>
        /// 添加列表陌生人的操作
        /// </summary>
        /// <returns>返回执行结果</returns>
        public string AddStranger(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int id = int.Parse(strs[0]);
            int strangerId = int.Parse(strs[1]);
            if (friendDAO.HasAdded(client.MySqlConnection, id, strangerId))
            {
                return ((int)ReturnCode.Fail).ToString() + "," + "已添加对方为好友";
            }
            else
            {
                string result = friendDAO.AddStranger(client.MySqlConnection, id, strangerId);
                if (result == " ")
                {
                    return ((int)ReturnCode.Success).ToString();
                }
                else
                {
                    return ((int)ReturnCode.Fail).ToString() + "," + result;
                }
            }
        }

        /// <summary>
        /// 同意添加好友
        /// </summary>
        /// <returns>返回反馈信息</returns>
        public string AgreeAddFriend(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int id = int.Parse(strs[0]);
            int friendId = int.Parse(strs[1]);
            if (friendDAO.AgreeAddFriend(client.MySqlConnection, id, friendId))
            {
                return ((int)ReturnCode.Success).ToString() + "," + friendId;
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString() + "," + friendId;
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
            int id = 0;
            int.TryParse(strs[0], out id);
            string nickName = strs[1];
            string ageOption = strs[2];
            string sexOption = strs[3];
            DataSet dataSet = null;
            if (id != 0 || nickName != "")
            {
                dataSet = friendDAO.BasicallySearch(client.MySqlConnection, id, nickName);
            }
            else if (ageOption != "" || sexOption != "")
            {
                dataSet = friendDAO.AdvancedSearch(client.MySqlConnection, ageOption, sexOption);
            }
            else
            {
                dataSet = friendDAO.RandomSearch(client.MySqlConnection);
            }

            if (dataSet != null && dataSet.Tables[0].Rows.Count != 0)
            {
                return ((int)ReturnCode.Success).ToString() + "," + DataHelper.GetStringFromTable(dataSet);
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
            Dictionary<int, (string, int)> friends;
            friends = friendDAO.GetFriends(client.MySqlConnection, int.Parse(data));
            if (friends != null && friends.Count != 0)
            {
                return ((int)ReturnCode.Success).ToString() + "," + DataHelper.DicToString(friends);
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        /// <summary>
        /// 获取好友的信息
        /// </summary>
        /// <returns>返回传送给客户端的信息</returns>
        public string GetFriendDetail(string data, Client client, Server server)
        {
            int id = int.Parse(data);
            User user = friendDAO.GetFriendUser(client.MySqlConnection, id);
            if (user == null)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                UserData userData = userDataDAO.GetUserDataByDataId(client.MySqlConnection, user.DataId);
                client.SetUserAndData(user, userData);
                return string.Format("{0},{1},{2}", ((int)ReturnCode.Success).ToString(), user.Id, userData.GetString());
            }
        }

        /// <summary>
        /// 更新陌生人列表请求
        /// </summary>
        /// <returns>返回陌生人的昵称和头像</returns>
        public string UpdateStrangerList(string data, Client client, Server server)
        {
            string result = "";
            result = friendDAO.UpdateStranger(client.MySqlConnection, int.Parse(data));
            if (result != "")
            {
                return ((int)ReturnCode.Success).ToString() + "," + result;
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
    }
}
