using System;
using System.Net.Sockets;


namespace XiaLM.Tcp.source.TcpEventArgs
{
    /// <summary>
    /// 与客户端建立连接事件参数
    /// </summary>
    public class TcpClientConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 终端
        /// </summary>
        public TcpClient TcpClient { get; set; }
        public TcpClientConnectedEventArgs(TcpClient tcp)
        {
            TcpClient = tcp;
        }
    }
}
