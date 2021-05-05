namespace ChatSystemServer.Servers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using ChatSystemServer.Controller;
    using Common;

    /// <summary>
    /// 用于处理客户端的连接.
    /// </summary>
    public class Server
    {
        private IPEndPoint _ipEndPoint;
        private Socket serverSocket;

        private ControllerManager controller;  // 单一控制实例

        private Dictionary<Client, int> onlineIdClients = new Dictionary<Client, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <param name="ip">传入ip</param>
        /// <param name="port">服务器使用的端口号</param>
        public Server(string ip, int port)
        {
            controller = new ControllerManager(this);
            this.SetIpAndPoint(ip, port);
        }

        /// <summary>
        /// 设置服务器的ipEndPoint
        /// </summary>
        /// <param name="ip">传入ip</param>
        /// <param name="port">服务器使用的端口号</param>
        public void SetIpAndPoint(string ip, int port)
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        /// <summary>
        /// 用于绑定ip，以及接收客户端连接
        /// </summary>
        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            serverSocket.Bind(_ipEndPoint);
            serverSocket.Listen(20);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        /// <summary>
        /// 对客户端的请求做出响应、处理
        /// </summary>
        public void RequestHander(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controller.RequestHander(requestCode, actionCode, data, client);
        }

        /// <summary>
        /// 对客户端发送的请求的处理结果传回，由client发送
        /// </summary>
        public void SendResponse(ActionCode actionCode, Client client, string data)
        {
            client.SnedReponse(actionCode, data);
        }

        /// <summary>
        /// 设置在线用户
        /// </summary>
        /// <param name="client">用户client</param>
        /// <param name="id">用户登录id</param>
        public void SetOnlineClient(Client client, int id)
        {
            onlineIdClients.Add(client, id);
        }

        /// <summary>
        /// 移除在线用户
        /// </summary>
        /// <param name="client">用户client</param>
        /// <param name="id">用户登录id</param>
        public void RemoveOnlineClient(Client client)
        {
            onlineIdClients.Remove(client);
        }

        /// <summary>
        /// 通过id获取好友的socket
        /// </summary>
        /// <returns>返回的socket</returns>
        public Client GetChatReceive(int id)
        {
            Client client = null;
            foreach (var item in onlineIdClients)
            {
                if (item.Value == id)
                {
                    client = item.Key;
                }
            }

            return client;
        }

        /// <summary>
        /// 异步通信，接收回调
        /// </summary>
        /// <param name="ar">接收异步操作接入的一个Client,并创建Socket处理</param>
        private void AcceptCallBack(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = serverSocket.EndAccept(ar);
                Client client = new Client(this, clientSocket);
                client.Start();
                serverSocket.BeginAccept(AcceptCallBack, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Start(); // 报错就重新开启
            }
        }
    }
}
