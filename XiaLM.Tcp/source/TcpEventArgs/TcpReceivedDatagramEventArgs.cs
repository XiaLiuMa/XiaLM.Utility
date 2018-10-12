using System;
using System.Net.Sockets;

namespace XiaLM.Tcp.source.TcpEventArgs
{
    /// <summary>
    /// 接收到数据报文事件参数
    /// </summary>
    /// <typeparam name="T">报文类型</typeparam>
    public class TcpReceivedDatagramEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient TcpClient { get; private set; }
        /// <summary>
        /// 报文
        /// </summary>
        public T Datagram { get; private set; }

        /// <summary>
        /// 接收到数据报文事件参数
        /// </summary>
        /// <param name="tcpClient">客户端</param>
        /// <param name="datagram">报文</param>
        public TcpReceivedDatagramEventArgs(TcpClient tcpClient, T datagram)
        {
            TcpClient = tcpClient;
            Datagram = datagram;
        } 
    }
}
