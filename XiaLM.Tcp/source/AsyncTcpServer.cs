using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using XiaLM.Tcp.source.DataParser;
using XiaLM.Tcp.source.TcpEventArgs;
using XiaLM.Tool450.source.common;

namespace XiaLM.Tcp.source
{
    /// <summary>
    /// 异步TCP服务器
    /// </summary>
    public class AsyncTcpServer : IDisposable
    {
        /// <summary>
        /// 解析器(粘包/分包)
        /// </summary>
        private IDataParser dataParser;
        /// <summary>
        /// 客户端字典<ip端口号，客户端信息>
        /// </summary>
        public Dictionary<string, TcpClient> clientDictionary;
        /// <summary>
        /// 服务器是否正在运行
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// 监听的IP地址
        /// </summary>
        public IPAddress Address { get; private set; }
        /// <summary>
        /// 监听的端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// 通信使用的编码
        /// </summary>
        public Encoding Encoding { get; set; }
        private TcpListener tcpListener;
        private bool disposed = false;

        /// <summary>
        /// 客户端建立连接事件
        /// </summary>
        public event EventHandler<TcpClientConnectedEventArgs> ClientConnected;
        /// <summary>
        /// 通知：客户端建立连接
        /// </summary>
        /// <param name="tcpClient"></param>
        private void NoticeClientConnected(TcpClient tcpClient)
        {
            if (ClientConnected != null)
            {
                ClientConnected(this, new TcpClientConnectedEventArgs(tcpClient));
            }
        }

        /// <summary>
        /// 客户端断开连接事件
        /// </summary>
        public event EventHandler<TcpClientDisconnectedEventArgs> ClientDisconnected;
        /// <summary>
        /// 通知：客户端断开连接
        /// </summary>
        /// <param name="tcpClient"></param>
        private void NoticeClientDisconnected(TcpClient tcpClient)
        {
            if (ClientDisconnected != null)
            {
                ClientDisconnected(this, new TcpClientDisconnectedEventArgs(tcpClient));
            }
        }

        /// <summary>
        /// 接收到客户端byte[]格式报文事件
        /// </summary>
        public event EventHandler<TcpReceivedDatagramEventArgs<byte[]>> DatagramReceived;
        /// <summary>
        /// 通知：接收到客户端byte[]格式的报文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="datagram"></param>
        private void NoticeDatagramReceived(TcpClient sender, byte[] datagram)
        {
            if (DatagramReceived != null)
            {
                DatagramReceived(this, new TcpReceivedDatagramEventArgs<byte[]>(sender, datagram));
            }
        }

        /// <summary>
        /// 异步TCP服务器(监听所有ip的port)
        /// </summary>
        /// <param name="listenPort">监听的端口</param>
        public AsyncTcpServer(int listenPort) : this(IPAddress.Any, listenPort) { }

        /// <summary>
        /// 异步TCP服务器(监听指定ip的port)
        /// </summary>
        /// <param name="localEP">监听的终结点</param>
        public AsyncTcpServer(IPEndPoint localEP) : this(localEP.Address, localEP.Port) { }

        /// <summary>
        /// 异步TCP服务器
        /// </summary>
        /// <param name="localIPAddress">监听的IP地址</param>
        /// <param name="listenPort">监听的port</param>
        private AsyncTcpServer(IPAddress localIPAddress, int listenPort)
        {
            Address = localIPAddress;
            Port = listenPort;
            this.Encoding = Encoding.Default;
            clientDictionary = new Dictionary<string, TcpClient>();
            tcpListener = new TcpListener(Address, Port);
            tcpListener.AllowNatTraversal(true);
            dataParser = new DataParserA();
        }

        /// <summary>
        /// 销毁服务器
        /// </summary>
        public void Dispose()
        {
            if (!this.disposed)
            {
                try
                {
                    Stop();
                    if (tcpListener != null)
                    {
                        tcpListener = null;
                    }
                    disposed = true;
                    GC.SuppressFinalize(this);
                }
                catch (SocketException ex)
                {
                    LogHelper.WriteError(ex);
                }
            }
        }

        /// <summary>
        /// 停止Tcp服务器
        /// </summary>
        /// <returns>异步TCP服务器</returns>
        public AsyncTcpServer Stop()
        {
            if (IsRunning)
            {
                tcpListener.Stop();
                IsRunning = false;
                lock (this.clientDictionary)
                {
                    foreach (var item in this.clientDictionary)
                    {
                        item.Value.Client.Disconnect(true);
                    }
                    this.clientDictionary.Clear();
                }
            }
            return this;
        }

        /// <summary>
        /// 启动Tcp服务器
        /// </summary>
        /// <param name="backlog">服务器所允许的挂起连接序列的最大长度/// </param>
        /// <returns>异步TCP服务器</returns>
        public AsyncTcpServer Start(int backlog = 0)
        {
            if (!IsRunning)
            {
                if (backlog != 0)
                {
                    tcpListener.Start(backlog);
                }
                else
                {
                    tcpListener.Start();
                }
                //监听新客户端
                tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnected), tcpListener);
                IsRunning = true;
            }
            return this;
        }

        /// <summary>
        /// 接收到新的客户端连接消息时回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void HandleClientConnected(IAsyncResult ar)
        {
            if (IsRunning)
            {
                TcpListener tcpListener = (TcpListener)ar.AsyncState;
                TcpClient tcpClient = tcpListener.EndAcceptTcpClient(ar);
                TcpClientEntity clientEntity = new TcpClientEntity(tcpClient);
                lock (this.clientDictionary)
                {
                    string clientId = GetRemoteIpEndPoint(tcpClient);
                    this.clientDictionary.Add(clientId, tcpClient);
                    NoticeClientConnected(tcpClient);
                }
                clientEntity.BeginReadCilentMsg();   //开始监听客户端
                clientEntity.ReturnColdPackageEvent += new Action<byte[]>((p) =>    //接收到客户端完整消息事件
                {
                    NoticeDatagramReceived(clientEntity.TcpClient, p);  
                });
                clientEntity.ClientDisconnectedEvent += new Action(() =>    //接收到客户端掉线事件
                {
                    lock (this.clientDictionary)
                    {
                        string clientId = GetRemoteIpEndPoint(clientEntity.TcpClient);
                        clientEntity.TcpClient.Client.Disconnect(true);
                        this.clientDictionary.Remove(clientId);
                        NoticeClientDisconnected(clientEntity.TcpClient);
                    }
                });
                tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnected), ar.AsyncState);//继续监听新客户端
            }
        }

        /// <summary>
        /// 发送报文至所有客户端
        /// </summary>
        /// <param name="bytes">报文</param>
        public void SendAll(byte[] bytes)
        {
            foreach (var item in this.clientDictionary)
            {
                Send(item.Value, bytes);
            }
        }

        /// <summary>
        /// 发送报文至指定的客户端
        /// </summary>
        /// <param name="tcpClient">客户端</param>
        /// <param name="bytes">报文</param>
        public void Send(TcpClient tcpClient, byte[] bytes)
        {
            if (IsRunning)
            {
                if (!IsRunning)
                    throw new InvalidProgramException("This TCP server has not been started.");

                if (tcpClient == null)
                    throw new ArgumentNullException("tcpClient");

                if (bytes == null)
                    throw new ArgumentNullException("datagram");
                byte[] dataBytes = dataParser.StickPackage(bytes);  //粘包
                tcpClient.GetStream().BeginWrite(dataBytes, 0, dataBytes.Length, HandleWriteDatagram, tcpClient);
            }
        }

        /// <summary>
        /// 写入数据到流中结束时回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void HandleWriteDatagram(IAsyncResult ar)
        {
            ((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);
        }

        /// <summary>
        /// 利用反射获取客户端ip和端口号，并为字符串
        /// </summary>
        /// <param name="cln"></param>
        /// <returns></returns>
        private string GetRemoteIpEndPoint(TcpClient cln)
        {
            PropertyInfo pi = cln.GetType().GetProperty("Client");
            Socket sock = (Socket)pi.GetValue(cln, null);
            string ipEndPoint = sock.RemoteEndPoint.ToString();
            return ipEndPoint;
        }
    }
}
