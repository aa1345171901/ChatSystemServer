
namespace ChatSystemServer.Model
{
    using System;

    /// <summary>
    /// 数据库表Messages
    /// </summary>
    public class Messages
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Messages"/> class.
        /// </summary>
        public Messages(int fromUserId, int toUserId, string message, DateTime sendTime)
        {
            FromUserId = fromUserId;
            ToUserId = toUserId;
            Message = message;
            SendTime = sendTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Messages"/> class.
        /// 默认构造
        /// </summary>
        public Messages()
        {
        }

        /// <summary>
        /// 消息id,自增
        /// </summary>
        public int MsgId { get; set; }

        /// <summary>
        /// 发送的用户id
        /// </summary>
        public int ToUserId { get; set; }

        /// <summary>
        /// 消息来自的用户id
        /// </summary>
        public int FromUserId { get; set; }

        /// <summary>
        /// 消息内容,系统消息为null
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 消息类型，1为好友消息，2为系统消息
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        /// 消息状态，0为未读，1为已读
        /// </summary>
        public int MessageState { get; set; }

        /// <summary>
        /// 消息发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 返回时间和信息内容的字符
        /// </summary>
        /// <returns>使用逗号分割</returns>
        public override string ToString()
        {
            DateTime epoc = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan delta = default(TimeSpan);
            delta = SendTime.Subtract(epoc);
            long ticks = (long)delta.TotalMilliseconds;
            return Message + "," + ticks;
        }
    }
}
