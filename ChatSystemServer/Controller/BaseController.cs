
namespace ChatSystemServer.Controller
{
    using Common;

    /// <summary>
    /// 抽象基类用于子类继承实现方法，方便通过RequestCode反射类
    /// </summary>
    public abstract class BaseController
    {
        /// <summary>
        /// 子类需要修改，所以设为protect不能时private
        /// </summary>
        protected RequestCode requestCode = RequestCode.None;

        /// <summary>
        /// 获取requestCode属性
        /// </summary>
        public RequestCode RequestCode
        {
            get { return requestCode; }
        }
    }
}
