using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        /// Base64加密(未传递编码类型默认采用utf8编码方式加密)
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(source, Encoding.UTF8);
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
            return Base64Decode(result, Encoding.UTF8);
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
