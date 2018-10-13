using System;
using System.Text;

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

        /// <summary>
        /// 将字符串Base64编码加密成字符串
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string ToBase64(string txt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(txt);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 将字节数组Base64编码加密成字符串
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string ToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64加密(未传递编码类型默认采用utf8编码方式加密)
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(source, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <returns></returns>
        public static string Base64Encode(string source, Encoding encodeType)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密(未传递编码类型默认采用utf8编码方式解密)
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result)
        {
            return Base64Decode(result, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result, Encoding encodeType)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

    }
}
