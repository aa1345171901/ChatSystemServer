
namespace ChatSystemServer.Model
{
    /// <summary>
    /// 对应数据库UserData
    /// </summary>
    public class UserData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserData"/> class.
        /// </summary>
        public UserData(int id, string nickName, string sex, int age, string name, int starId, int bloodTypeId, int faceId)
        {
            Id = id;
            NickName = nickName;
            Sex = sex;
            Age = age;
            Name = name;
            StarId = starId;
            BloodTypeId = bloodTypeId;
            FaceId = faceId;
        }

        /// <summary>
        /// 与user表的dataid对应
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 设置的性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 星座,对应数据库Star表
        /// </summary>
        public int StarId { get; set; }

        /// <summary>
        /// 血型,对应数据库BloodType表
        /// </summary>
        public int BloodTypeId { get; set; }

        /// <summary>
        /// 头像Id
        /// </summary>
        public int FaceId { get; set; }

        /// <summary>
        /// 返回需要传送到客户端的属性的格式
        /// </summary>
        /// <returns>返回字符串使用'，'分割</returns>
        public string GetString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", Id, NickName, Sex, Age, Name, StarId, BloodTypeId, FaceId);
        }
    }
}
