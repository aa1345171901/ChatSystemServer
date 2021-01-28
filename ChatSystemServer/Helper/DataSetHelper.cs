
namespace ChatSystemServer.Helper
{
    using System.Data;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// 用于将DataSet转换成byte数组，或者数组转换成DataSet数据集
    /// </summary>
    public class DataSetHelper
    {
        /// <summary>
        /// 将DataSet转byte数组
        /// </summary>
        /// <returns>返回转换后的Byte数组</returns>
        public static byte[] GetBinaryFormatDataSet(DataSet ds)
        {
            // 创建内存流
            MemoryStream memStream = new MemoryStream();

            // 产生二进制序列化格式
            IFormatter formatter = new BinaryFormatter();

            // 指定DataSet串行化格式是二进制
            ds.RemotingFormat = SerializationFormat.Binary;

            // 串行化到内存中
            formatter.Serialize(memStream, ds);

            // 将DataSet转化成byte[]
            byte[] binaryResult = memStream.ToArray();

            // 清空和释放内存流
            memStream.Close();
            memStream.Dispose();
            return binaryResult;
        }

        /// <summary>
        /// 将byte数组转换成DataSet
        /// </summary>
        /// <returns>转换后的DataSet</returns>
        public static DataSet RetrieveDataSet(byte[] binaryData)
        {
            // 创建内存流
            MemoryStream memStream = new MemoryStream(binaryData);

            memStream.Seek(0, SeekOrigin.Begin);

            // 产生二进制序列化格式
            IFormatter formatter = new BinaryFormatter();

            // 反串行化到内存中
            object obj = formatter.Deserialize(memStream);

            // 类型检验
            if (obj is DataSet)
            {
                DataSet dataSetResult = (DataSet)obj;
                return dataSetResult;
            }
            else
            {
                return null;
            }
        }
    }
}
