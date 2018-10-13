using System;
using System.Security.Cryptography;
using System.Text;

namespace XiaLM.Tool450.source.common
{
    /// <summary>
    /// 加密助手
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// 生成特定位数的唯一字符串([0-9,A-Z,a-z] + Guid.NewGuid())
        /// </summary>
        /// <param name="num">特定位数</param>
        /// <returns></returns>
        public static string GenerateUniqueText(int num)
        {
            string randomResult = string.Empty;
            string readyStr = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] rtn = new char[num];
            Guid gid = Guid.NewGuid();
            var ba = gid.ToByteArray();
            for (var i = 0; i < num; i++)
            {
                rtn[i] = readyStr[((ba[i] + ba[num + i]) % 35)];
            }
            foreach (char r in rtn)
            {
                randomResult += r;
            }
            return randomResult;
        }

        /// <summary>
        /// MD5码加密字符串(32位小写)
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string Md5Encryp(string txt)
        {
            string result = string.Empty;
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(txt));
            for (int i = 0; i < bytes.Length; i++)
            {
                result += bytes[i].ToString("x2");//每个元素进行十六进制转换然后拼接成s字符串
            }
            return result;
        }

        /// <summary>
        /// 获取 utc 1970-1-1到现在的秒数
        /// </summary>
        /// <returns></returns>
        public static long Get1970ToNowSeconds()
        {
            return (DateTime.UtcNow.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>
        /// 获取 utc 1970-1-1到现在的毫秒数
        /// </summary>
        /// <returns></returns>
        public static long Get1970ToNowMilliseconds()
        {
            return (DateTime.UtcNow.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }

        ///// <summary>
        ///// 加密
        ///// </summary>
        ///// <param name="Text"></param>
        ///// <returns></returns>
        //public static string Encrypt(string Text)
        //{
        //    return Encrypt(Text, "xinniuren.cn2017");
        //}
        ///// <summary> 
        ///// 加密数据 
        ///// </summary> 
        ///// <param name="Text"></param> 
        ///// <param name="sKey"></param> 
        ///// <returns></returns> 
        //public static string Encrypt(string Text, string sKey)
        //{
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    byte[] inputByteArray;
        //    inputByteArray = Encoding.Default.GetBytes(Text);
        //    des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        //    des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        //    cs.Write(inputByteArray, 0, inputByteArray.Length);
        //    cs.FlushFinalBlock();
        //    StringBuilder ret = new StringBuilder();
        //    foreach (byte b in ms.ToArray())
        //    {
        //        ret.AppendFormat("{0:X2}", b);
        //    }
        //    return ret.ToString();
        //}

        ///// <summary>
        ///// 解密
        ///// </summary>
        ///// <param name="Text"></param>
        ///// <returns></returns>
        //public static string Decrypt(string Text)
        //{
        //    return Decrypt(Text, "xinniuren.cn2017");
        //}
        ///// <summary> 
        ///// 解密数据 
        ///// </summary> 
        ///// <param name="Text"></param> 
        ///// <param name="sKey"></param> 
        ///// <returns></returns> 
        //public static string Decrypt(string Text, string sKey)
        //{
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    int len;
        //    len = Text.Length / 2;
        //    byte[] inputByteArray = new byte[len];
        //    int x, i;
        //    for (x = 0; x < len; x++)
        //    {
        //        i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
        //        inputByteArray[x] = (byte)i;
        //    }
        //    des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        //    des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
        //    cs.Write(inputByteArray, 0, inputByteArray.Length);
        //    cs.FlushFinalBlock();
        //    return Encoding.Default.GetString(ms.ToArray());
        //}
    }
}
