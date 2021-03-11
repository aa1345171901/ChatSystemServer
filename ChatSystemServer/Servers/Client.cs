namespace ChatSystemServer.Servers
{
    using System;
    using System.Net.Sockets;
    using System.Threading;
    using ChatSystemServer.Helper;
    using ChatSystemServer.Model;
    using Common;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// 用于处理连接进来的客户端
    /// </summary>
    public class Client
    {
        private Socket clientSocket;
        private Server serverSocket;

        private MySqlConnection mySqlConnection;

        private Message message = new Message();

        private User user;
        private UserData userData;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// 初始化Client
        /// </summary>
        /// <param name="serverSocket">创建Client时,将server绑定在一起</param>
        /// <param name="clientSocket">服务器使用的端口号</param>
        public Client(Server serverSocket, Socket clientSocket)
        {
            mySqlConnection = DBHelper.Connect();
            Console.WriteLine("一个客户端连入");
            this.clientSocket = clientSocket;
            this.serverSocket = serverSocket;
        }

        /// <summary>
        /// 返回该Socket的数据库连接
        /// </summary>
        public MySqlConnection MySqlConnection
        {
            get { return mySqlConnection; }
        }

        /// <summary>
        /// 设置User和UserData与该client对象绑定
        /// </summary>
        public void SetUserAndData(User user, UserData userData)
        {
            this.user = user;
            this.userData = userData;
        }

        /// <summary>
        /// 发送响应是否成功以及响应信息给客户端
        /// </summary>
        public void SnedReponse(ActionCode actionCode, string data)
        {
            byte[] bytes = Message.PackData(actionCode, data);
            try
            {
                clientSocket.Send(bytes);
            }
            catch (Exception e)
            {
                Close();
                Console.WriteLine("发送给客户端回应失败" + e.Message);
            }
        }

        /// <summary>
        /// 用于开启客户端的服务
        /// </summary>
        public void Start()
        {
            clientSocket.BeginReceive(message.Data, message.StartIndex, message.RemainSize, SocketFlags.None, ReceiveCallBack, null);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    int count = clientSocket.EndReceive(ar);
                    if (count <= 0)
                    {
                        Close();
                    }

                    message.ReadMessage(count, OnProcessMessage);
                    Start();
                }
                else
                {
                    Close();
                }
            }
            catch (Exception e)
            {
                Close();
                Console.WriteLine("接收失败:" + e.Message);
            }
        }

        /// <summary>
        /// 对解析出的消息传递给server然后执行相应操作
        /// </summary>
        private void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            try
            {
                serverSocket.RequestHander(requestCode, actionCode, data, this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// 当客户端没有请求时，用于关闭连接
        /// </summary>
        private void Close()
        {
            DBHelper.Close(mySqlConnection);
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                Thread.Sleep(10);
            }

            if (clientSocket != null)
            {
                clientSocket.Close();
            }
        }
    }
}
