using System;
using System.Collections.Generic;


namespace XiaLM.Tcp.source.DataParser
{
    public class DataParserA : IDataParser
    {
        private bool isEnd = true; //上一次拆包是否是完整的包(该包是否还有后续数据未接收完?)
        //private byte[] tempBytes = null;    //临时记录上次不完整的数据
        private byte[] _head = new byte[2] { 0xEA, 0x56 };  //包头标识
        public event Action<byte[]> ReturnUnPackageBytes = p => { };

        /// <summary>
        /// 校验包头标识
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        private bool IsHead(byte[] bs)
        {
            if (bs.Length != 2) return false;
            var b = true;
            int i = 0;
            foreach (var item in _head)
            {
                if (!item.Equals(bs[i]))
                    b = false;
                i++;
            }
            return b;
        }

        /// <summary>
        /// CRC16校验
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private ushort CRC16_CCITT(byte[] buffer)
        {
            ushort crc = 0x0000;
            foreach (var b in buffer)
            {
                crc = (ushort)(crc ^ (b));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0x8408) : (ushort)(crc >> 1);
                }
            }
            return (ushort)(crc ^ 0x0000);
        }

        /// <summary>
        /// 粘包
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] StickPackage(byte[] data)
        {
            try
            {
                byte[] headBytes = new byte[2] { 0xEA, 0x56 };   //数据头标识  
                byte[] lenthBytes = BitConverter.GetBytes(data.Length); //数据长度
                ushort crc16Code = CRC16_CCITT(data);  //校验码
                byte[] crc16Bytes = BitConverter.GetBytes(crc16Code);
                byte[] result = new byte[8 + data.Length];    //返回数据
                Array.Copy(headBytes, 0, result, 0, 2);
                Array.Copy(lenthBytes, 0, result, 2, 4);
                Array.Copy(data, 0, result, 6, data.Length);
                Array.Copy(crc16Bytes, 0, result, data.Length + 6, 2);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 拆包，只有完整的包才返回
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<byte[]> UnPackage(byte[] data)
        {
            List<byte[]> result = new List<byte[]>();
            if (isEnd)
            {
                if (data.Length < 8) return null;   //小于8无效
                byte[] headBytes = new byte[2];
                Array.Copy(data, 0, headBytes, 0, 2);
                if (!IsHead(headBytes)) return null;
                byte[] lenthBytes = new byte[4];
                Array.Copy(data, 2, lenthBytes, 0, 4);
                int lenth = BitConverter.ToInt32(lenthBytes, 0);
                if (data.Length < lenth + 8)    //当前包未接收完
                {
                    isEnd = false;
                }
                if (data.Length == lenth + 8)   //当前包刚好接收完
                {
                    isEnd = true;
                }
                if (data.Length > lenth + 8)   //当前包含有多个包
                {
                    isEnd = true;
                }
                byte[] dataBytes = new byte[lenth];
                Array.Copy(data, 6, dataBytes, 0, lenth);
                byte[] crc16Bytes = new byte[2];
                Array.Copy(data, lenth + 6, crc16Bytes, 0, 2);
                if (BitConverter.ToInt16(crc16Bytes, 0) != CRC16_CCITT(dataBytes)) return null;

                return null;
            }
            else
            {
                return null;
            }
        }

    }
}
