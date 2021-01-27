
namespace ChatSystemServer.Controller
{
    using ChatSystemServer.DAO;
    using ChatSystemServer.Model;
    using ChatSystemServer.Servers;
    using Common;

    /// <summary>
    /// 用于处理用户信息处理的控制器
    /// </summary>
    public class UserController : BaseController
    {
        private UserDAO userDAO;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// 更改RequestCode
        /// </summary>
        public UserController()
        {
            requestCode = RequestCode.User;
            userDAO = new UserDAO();
        }

        /// <summary>
        /// 用于处理登录操作
        /// </summary>
        /// <returns>返回操作成功与否以及用户信息</returns>
        public string Login(string data, Client client, Server server)
        {
            int id = int.Parse(data.Split(',')[0]);
            string password = data.Split(',')[1];
            User user = userDAO.VerifyUser(client.MySqlConnection, id, password);
            if (user == null)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                UserData userData = userDAO.GetUserDataByDataId(client.MySqlConnection, user.DataId);
                client.SetUserAndData(user, userData);
                return string.Format("{0},{1},{2}", ((int)ReturnCode.Success).ToString(), user.Id, userData.GetString());
            }
        }

        /// <summary>
        /// 处理客户端的注册功能
        /// </summary>
        /// <returns>返回操作是否成功，以及用户id</returns>
        public string Register(string data, Client client, Server server)
        {
            string nickName = data.Split(',')[0];
            string password = data.Split(',')[1];
            User user = userDAO.AddUser(client.MySqlConnection, nickName, password);
            if (user == null)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                return ((int)ReturnCode.Success).ToString() + "," + user.Id;
            }
        }

        /// <summary>
        /// 注册后选填信息
        /// </summary>
        /// <returns>返回给客户端保存是否成功的操作</returns>
        public string Optional(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int dataId = int.Parse(strs[0]);
            string sex = strs[1];
            int age = int.Parse(strs[2]);
            string name = strs[3];
            int starid = int.Parse(strs[4]);
            int bloodtypeid = int.Parse(strs[5]);
            if (userDAO.Optional(client.MySqlConnection, dataId, sex, age, name, starid, bloodtypeid))
            {
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <returns>返回操作是否成功</returns>
        public string Modify(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int dataId = int.Parse(strs[0]);
            string nickName = strs[1];
            string sex = strs[2];
            int age = int.Parse(strs[3]);
            string name = strs[4];
            int starid = int.Parse(strs[5]);
            int bloodtypeid = int.Parse(strs[6]);
            int faceId = int.Parse(strs[7]);
            if (userDAO.ModifyById(client.MySqlConnection, dataId, nickName, sex, age, name, starid, bloodtypeid, faceId))
            {
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
    }
}
