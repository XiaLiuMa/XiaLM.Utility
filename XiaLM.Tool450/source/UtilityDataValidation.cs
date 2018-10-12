using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tool450.source
{
    /// <summary>
    /// 数据校验
    /// </summary>
    public class UtilityDataValidation
    {
        /// <summary>
        /// CRC16校验
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static ushort CRC16_CCITT(byte[] buffer)
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
    }
}
