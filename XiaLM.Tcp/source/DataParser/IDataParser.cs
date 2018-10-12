using System;
using System.Collections.Generic;


namespace XiaLM.Tcp.source.DataParser
{
    /// <summary>
    /// 解析器接口
    /// </summary>
    public interface IDataParser
    {
        /// <summary>
        /// 粘包
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] StickPackage(byte[] data);
        /// <summary>
        /// 拆包
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        List<byte[]> UnPackage(byte[] data);
        /// <summary>
        /// 返回拆包完整数据报
        /// </summary>
        event Action<byte[]> ReturnUnPackageBytes;
    }
}
