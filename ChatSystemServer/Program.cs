
namespace ChatSystemServer
{
    using System;
    using ChatSystemServer.Servers;

    /// <summary>
    /// 程序的入口.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 程序入口.
        /// </summary>
        /// <param name="args">传入的参数</param>
        private static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 8888);
            server.Start();
            while (true)
            {
                Console.Read();
            }
        }
    }
}
