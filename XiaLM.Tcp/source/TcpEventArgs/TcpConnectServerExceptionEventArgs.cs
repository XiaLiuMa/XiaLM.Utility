using System;
using System.Globalization;
using System.Net;


namespace XiaLM.Tcp.source.TcpEventArgs
{
    /// <summary>
    /// 连接服务器异常事件参数
    /// </summary>
    public class TcpConnectServerExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 服务器IP地址列表
        /// </summary>
        public IPAddress[] Addresses { get; private set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// 内部异常
        /// </summary>
        public Exception Exception { get; private set; }

        public TcpConnectServerExceptionEventArgs(IPAddress[] ipAddresses, int port, Exception innerException)
        {
            this.Addresses = ipAddresses;
            this.Port = port;
            this.Exception = innerException;
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = string.Empty;
            if (Addresses != null && Addresses.Length > 0)
            {
                foreach (var item in Addresses)
                {
                    s = s + item.ToString() + ',';
                }
                s = s.TrimEnd(',');
                s = s + ":" + Port.ToString(CultureInfo.InvariantCulture);
            }
            return s;
        }
    }
}
