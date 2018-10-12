using System;
using System.Globalization;
using System.Net;


namespace XiaLM.Tcp.source.TcpEventArgs
{
    /// <summary>
    /// 与服务器建立连接事件参数
    /// </summary>
    public class TcpServerConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 服务器IP地址列表
        /// </summary>
        public IPAddress[] Addresses { get; private set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; private set; }

        public TcpServerConnectedEventArgs(IPAddress[] ipAddresses, int port)
        {
            this.Addresses = ipAddresses;
            this.Port = port;
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
