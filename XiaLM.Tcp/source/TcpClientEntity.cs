using System;
using System.Collections.Generic;
using System.Net.Sockets;
using XiaLM.Tcp.source.DataParser;

namespace XiaLM.Tcp.source
{
    /// <summary>
    /// 客户端实体
    /// </summary>
    public class TcpClientEntity
    {
        /// <summary>
        /// 解析器
        /// </summary>
        public IDataParser dataParser;
        /// <summary>
        /// 终端
        /// </summary>
        public TcpClient TcpClient { get; private set; }
        /// <summary>
        /// 返回客户端完整数据报事件
        /// </summary>
        public event Action<byte[]> ReturnColdPackageEvent = p => { };
        /// <summary>
        /// 客户端下线事件
        /// </summary>
        public event Action ClientDisconnectedEvent = () => { };

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="buffer"></param>
        public TcpClientEntity(TcpClient tcpClient)
        {
            this.TcpClient = tcpClient;
            dataParser = new DataParserA();
        }

        /// <summary>
        /// 开始监听客户端消息
        /// </summary>
        public void BeginReadCilentMsg()
        {
            ClientState clientState = new ClientState(this.TcpClient);
            clientState.NetworkStream.BeginRead(clientState.Buffer, 0, clientState.Buffer.Length, HandleDatagramReceived, clientState);
        }

        /// <summary>
        /// 接收到匹配的客户端数据时回掉函数
        /// </summary>
        /// <param name="ar"></param>
        private void HandleDatagramReceived(IAsyncResult ar)
        {
            if (true)
            {
                ClientState clientState = (ClientState)ar.AsyncState;
                NetworkStream networkStream = clientState.NetworkStream;
                int numberOfReadBytes = 0;  //接收到的数据字节长度
                try
                {
                    numberOfReadBytes = networkStream.EndRead(ar);
                }
                catch
                {
                    numberOfReadBytes = 0;
                }
                if (numberOfReadBytes == 0) //接收到的数据为0时，结束通信
                {
                    ClientDisconnectedEvent();
                    return;
                }
                byte[] receivedBytes = new byte[numberOfReadBytes];
                Array.Copy(clientState.Buffer, 0, receivedBytes, 0, numberOfReadBytes);
                List<byte[]> packages = dataParser.UnPackage(receivedBytes);
                if (packages != null && packages.Count > 0)
                {
                    foreach (var item in packages)
                    {
                        ReturnColdPackageEvent(item);
                    }
                }

                networkStream.BeginRead(clientState.Buffer, 0, clientState.Buffer.Length, HandleDatagramReceived, clientState); //继续 读取客户端消息
            }
        }
    }

    /// <summary>
    /// 接收客户端消息实体
    /// </summary>
    public class ClientState
    {
        /// <summary>
        /// 终端
        /// </summary>
        public TcpClient TcpClient { get; private set; }
        /// <summary>
        /// 网络数据流
        /// </summary>
        public NetworkStream NetworkStream { get; private set; }
        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Buffer { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="buffer"></param>
        public ClientState(TcpClient tcpClient)
        {
            this.TcpClient = tcpClient;
            this.Buffer = new byte[tcpClient.ReceiveBufferSize];
            this.NetworkStream = tcpClient.GetStream();
        }
    }

}
