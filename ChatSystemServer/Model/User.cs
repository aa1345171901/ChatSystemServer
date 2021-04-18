namespace ChatSystemServer.Model
{
    /// <summary>
    /// 对应数据库user的各项属性
    /// </summary>
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">创建User对象时传递的id</param>
        /// <param name="passWord">创建User对象时传递的password</param>
        public User(int id, string passWord, int dataId)
        {
            Id = id;
            PassWord = passWord;
            DataId = dataId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// 构造函数，用于获取好友dataid
        /// </summary>
        public User(int id, int dataId)
        {
            Id = id;
            DataId = dataId;
        }

        /// <summary>
        /// 用户的id，相当于账号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户的密码，用于验证登录
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 用于连接外表userdata
        /// </summary>
        public int DataId { get; set; }
    }
}
