using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tool450.source.common
{
    /// <summary>
    /// Base64编码解码助手
    /// </summary>
    public class Base64Helper
    {
        /// <summary>
        /// 编码：字节数组转base64字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encoding(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// 解码：base64字符串转字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] Decoding(string str)
        {
            return Convert.FromBase64String(str);
        }
    }
}
