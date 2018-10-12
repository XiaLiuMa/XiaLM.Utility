using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tool450.source.tcp
{
    /// <summary>
    /// 解码委托，回掉函数
    /// </summary>
    /// <param name="bytes"></param>
    public delegate void DataDecodResults(byte[] bytes);
    /// <summary>
    /// 数据解析器接口
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// 编码(粘包)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Encoding(byte[] data);
        /// <summary>
        /// 编码(拆包)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        void Decoding(byte[] data, DataDecodResults callback);
    }
}
