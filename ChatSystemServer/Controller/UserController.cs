
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
        /// <returns>返回操作成功与否</returns>
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
    }
}
