
namespace ChatSystemServer.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Reflection;
    using ChatSystemServer.Servers;
    using Common;

    /// <summary>
    /// 用于管理各种控制器，解析出ActionCode的方法并执行
    /// </summary>
    public class ControllerManager
    {
        private Server serverSocket;
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerManager"/> class.
        /// </summary>
        public ControllerManager(Server server)
        {
            serverSocket = server;
        }

        /// <summary>
        /// 将controller加入字典
        /// </summary>
        public void InitController()
        {
            controllerDict.Add(RequestCode.User, new UserController());
        }

        /// <summary>
        /// 用于解析出ActionCode并通过Controller执行相应方法
        /// </summary>
        public void RequestHander(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            BaseController baseController;
            bool isGet = controllerDict.TryGetValue(requestCode, out baseController);
            if (isGet == false)
            {
                Console.WriteLine("无法得到[" + requestCode + "]所对应的Conrtoller，无法处理");
            }

            // 通过这种方法才能获得枚举的名字，使用toString得到的时数字
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);

            // 通过方法名反射出子类的方法
            MethodInfo info = baseController.GetType().GetMethod(methodName);

            if (info == null)
            {
                Console.WriteLine("无法得到[" + actionCode + "]所对应的方法，无法处理");
            }

            object[] parameters = new object[] { data, client, serverSocket }; // 方法的参数
            object rt = info.Invoke(baseController, parameters); // 调用方法的返回值，用于回传给客户端
            if (rt == null || string.IsNullOrEmpty(rt as string))
            {
                return;
            }

            return 
        }
    }
}
