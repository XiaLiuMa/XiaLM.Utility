using System;
using System.Net.Sockets;


namespace XiaLM.Tcp.source.TcpEventArgs
{
    /// <summary>
    /// 与客户端断开连接事件参数
    /// </summary>
    public class TcpClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 终端
        /// </summary>
        public TcpClient TcpClient { get; set; }
        public TcpClientDisconnectedEventArgs(TcpClient tcp)
        {
            TcpClient = tcp;
        }
    }
}
