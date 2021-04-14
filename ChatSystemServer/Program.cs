
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
            // string path = System.IO.Directory.GetCurrentDirectory();
            // DateTime time = DateTime.Now;
            // DateTime epoc = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            // TimeSpan delta = default(TimeSpan);
            // delta = time.Subtract(epoc);
            // long ticks = (long)delta.TotalMilliseconds;
            // var date = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            // date = date.AddMilliseconds(ticks);
            Server server = new Server("172.22.79.220", 8888);  // 172.22.79.220
            server.Start();
            while (true)
            {
                Console.Read();
            }
        }
    }
}
