using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tool450.source.common
{
    /// <summary>
    /// 数据类型转换助手
    /// </summary>
    public class ConvertHelper
    {
        /// <summary>
        /// IntPtr转string
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string IntPtrToString(IntPtr p, string encoding = "UTF-8")
        {
            List<byte> bs = new List<byte>();
            while (Marshal.ReadByte(p) != 0)
            {
                bs.Add(Marshal.ReadByte(p));
                p = p + 1;
            }
            return Encoding.GetEncoding(encoding).GetString(bs.ToArray());
        }

        /// <summary>
        /// byte[]转IntPtr
        /// </summary>
        /// <param name="bs">byte[]</param>
        /// <returns></returns>
        public static IntPtr BytesToIntPtr(byte[] bs)
        {
            return Marshal.UnsafeAddrOfPinnedArrayElement(bs, 0);
        }

        /// <summary>
        /// String转Int
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int StringToInt(string s)
        {
            return BitConverter.ToInt32(Encoding.Default.GetBytes(s), 0);
        }

        /// <summary>
        /// IntPtr转字节数组
        /// </summary>
        /// <param name="p"></param>
        /// <param name="arrayLen">数组长度</param>
        /// <returns></returns>
        public static byte[] IntPtrToBytes(IntPtr p, int arrayLen)
        {
            byte[] bs = new byte[arrayLen];
            Marshal.Copy(p, bs, 0, arrayLen);
            return bs;
        }
        /// <summary>
        /// Struct转byte[]
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        public static byte[] StructToBytes(object structure)
        {
            int num = Marshal.SizeOf(structure);
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            byte[] result;
            try
            {
                Marshal.StructureToPtr(structure, intPtr, false);
                byte[] array = new byte[num];
                Marshal.Copy(intPtr, array, 0, num);
                result = array;
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }
            return result;
        }
        /// <summary>
        /// Struct转IntPtr
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        public static IntPtr StructureToIntPtr(object structure)
        {
            int num = Marshal.SizeOf(structure);
            IntPtr intPtr = Marshal.AllocCoTaskMem(num);
            Marshal.StructureToPtr(structure, intPtr, false);
            return intPtr;
        }

        public static void Release(IntPtr intPtr)
        {
            Marshal.FreeHGlobal(intPtr);
        }
        public static T IntPtrToStructure<T>(IntPtr p)
        {
            return (T)Marshal.PtrToStructure(p, typeof(T));
        }


        /// <summary>  
        /// 字符串转Unicode  
        /// </summary>  
        /// <param name="source">源字符串</param>  
        /// <returns>Unicode编码后的字符串</returns>  
        public static string String2Unicode(string source)
        {
            var bytes = Encoding.Unicode.GetBytes(source);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0:x2}{1:x2}", bytes[i + 1], bytes[i]);
            }
            return stringBuilder.ToString();
        }
        /// <summary>    
        /// 字符串转为UniCode码字符串    
        /// </summary>    
        /// <param name="s"></param>    
        /// <returns></returns>    
        public static string StringToUnicode(string s)
        {
            char[] charbuffers = s.ToCharArray();
            byte[] buffer;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charbuffers.Length; i++)
            {
                buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                sb.Append(String.Format("\\u{0:X2}{1:X2}", buffer[1], buffer[0]));
            }
            return sb.ToString();
        }
        /// <summary>    
        /// Unicode字符串转为正常字符串    
        /// </summary>    
        /// <param name="srcText"></param>    
        /// <returns></returns>    
        public static string UnicodeToString(string srcText)
        {
            string dst = "";
            string src = srcText;
            int len = srcText.Length / 6;
            for (int i = 0; i <= len - 1; i++)
            {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;
        }
    }
}
