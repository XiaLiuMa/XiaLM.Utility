using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XiaLM.Tcp.source.DataParser;
using XiaLM.Tcp.source.TcpEventArgs;
using XiaLM.Tool450.source.common;

namespace XiaLM.Tcp.source
{
    /// <summary>
    /// 异步TCP客户端
    /// </summary>
    public class AsyncTcpClient : IDisposable
    {
        public TcpClient tcpClient;
        /// <summary>
        /// 用于解析接收数据的缓冲池
        /// </summary>
        private Queue<byte> receiveBuffer = new Queue<byte>();
        /// <summary>
        /// 解析器(粘包/分包)
        /// </summary>
        private IDataParser dataParser;
        /// <summary>
        /// 本地客户端终结点(可作为唯一标识)
        /// </summary>
        public IPEndPoint LocalIPEndPoint { get; private set; }
        /// <summary>
        /// (计数用的)重连次数
        /// </summary>
        private int retries = 0;
        /// <summary>
        /// 是否已销毁
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// 是否已与服务器建立连接
        /// </summary>
        public bool Connected { get { return tcpClient.Client.Connected; } }
        /// <summary>
        /// 远端服务器的IP地址列表
        /// </summary>
        public IPAddress[] Addresses { get; private set; }
        /// <summary>
        /// 远端服务器的端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// 连接重试次数
        /// </summary>
        public int Retries { get; set; }
        /// <summary>
        /// 连接重试间隔
        /// </summary>
        public int RetryInterval { get; set; }
        /// <summary>
        /// 远端服务器终结点
        /// </summary>
        public IPEndPoint RemoteIPEndPoint { get { return new IPEndPoint(Addresses[0], Port); } }
        /// <summary>
        /// 通信所使用的编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 通知：与服务器建立连接事件
        /// </summary>
        /// <param name="ipAddresses"></param>
        /// <param name="port"></param>
        private void RaiseServerConnected(IPAddress[] ipAddresses, int port)
        {
            if (ServerConnected != null)
            {
                ServerConnected(this, new TcpServerConnectedEventArgs(ipAddresses, port));
            }
        }
        /// <summary>
        /// 与服务器建立连接事件
        /// </summary>
        public event EventHandler<TcpServerConnectedEventArgs> ServerConnected;

        /// <summary>
        /// 通知：与服务器断开连接事件
        /// </summary>
        /// <param name="ipAddresses"></param>
        /// <param name="port"></param>
        private void RaiseServerDisconnected(IPAddress[] ipAddresses, int port)
        {
            if (ServerDisconnected != null)
            {
                ServerDisconnected(this, new TcpServerDisconnectedEventArgs(ipAddresses, port));
            }
        }
        /// <summary>
        /// 与服务器的连接已断开事件
        /// </summary>
        public event EventHandler<TcpServerDisconnectedEventArgs> ServerDisconnected;

        /// <summary>
        /// 通知：与服务器连接异常事件
        /// </summary>
        /// <param name="ipAddresses"></param>
        /// <param name="port"></param>
        /// <param name="innerException"></param>
        private void RaiseServerExceptionOccurred(IPAddress[] ipAddresses, int port, Exception innerException)
        {
            if (ServerExceptionOccurred != null)
            {
                ServerExceptionOccurred(this, new TcpConnectServerExceptionEventArgs(ipAddresses, port, innerException));
            }
        }
        /// <summary>
        /// 与服务器连接异常事件
        /// </summary>
        public event EventHandler<TcpConnectServerExceptionEventArgs> ServerExceptionOccurred;

        /// <summary>
        /// 通知：接收到数据报文事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="datagram"></param>
        private void RaiseDatagramReceived(TcpClient sender, byte[] datagram)
        {
            if (DatagramReceived != null)
            {
                DatagramReceived(this, new TcpReceivedDatagramEventArgs<byte[]>(sender, datagram));
            }
        }
        /// <summary>
        /// 接收到数据报文事件
        /// </summary>
        public event EventHandler<TcpReceivedDatagramEventArgs<byte[]>> DatagramReceived;

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteEP">远端服务器终结点</param>
        public AsyncTcpClient(IPEndPoint remoteEP) : this(new[] { remoteEP.Address }, remoteEP.Port) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="rEP">远端服务器终结点</param>
        /// <param name="lEP">本地客户端终结点</param>
        public AsyncTcpClient(IPEndPoint rEP, IPEndPoint lEP) : this(new[] { rEP.Address }, rEP.Port, lEP) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="address">远端服务器IP地址</param>
        /// <param name="port">远端服务器端口</param>
        public AsyncTcpClient(IPAddress address, int port) : this(new[] { address }, port) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="address">远端服务器IP地址</param>
        /// <param name="port">远端服务器端口</param>
        /// <param name="localEP">本地客户端终结点</param>
        public AsyncTcpClient(IPAddress address, int port, IPEndPoint localEP) : this(new[] { address }, port, localEP) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="rHostName">远端服务器主机名</param>
        /// <param name="rPort">远端服务器端口</param>
        public AsyncTcpClient(string rHostName, int rPort) : this(Dns.GetHostAddresses(rHostName), rPort) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteHostName">远端服务器主机名</param>
        /// <param name="remotePort">远端服务器端口</param>
        /// <param name="lEP">本地客户端终结点</param>
        public AsyncTcpClient(string rName, int rPort, IPEndPoint lEP) : this(Dns.GetHostAddresses(rName), rPort, lEP) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="rAddresses">远端服务器IP地址列表</param>
        /// <param name="rPort">远端服务器端口</param>
        public AsyncTcpClient(IPAddress[] rAddresses, int rPort) : this(rAddresses, rPort, null) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="rAddresses">远端服务器IP地址列表</param>
        /// <param name="rPort">远端服务器端口</param>
        /// <param name="localEP">本地客户端终结点</param>
        public AsyncTcpClient(IPAddress[] rAddresses, int rPort, IPEndPoint localEP)
        {
            this.Addresses = rAddresses;
            this.Port = rPort;
            this.LocalIPEndPoint = localEP;
            this.Encoding = Encoding.Default;
            if (this.LocalIPEndPoint != null)
            {
                this.tcpClient = new TcpClient(this.LocalIPEndPoint);
            }
            else
            {
                this.tcpClient = new TcpClient();
            }
            this.Retries = 3;
            this.RetryInterval = 5;
            dataParser = new DataParserA();
            dataParser.ReturnUnPackageBytes += DataParser_ReturnUnPackageBytes;

            ParserReceiveDate();    //解析接收的数据
        }

        /// <summary>
        /// 销毁客户端
        /// </summary>
        public void Dispose()
        {
            if (!this.disposed)
            {
                try
                {
                    Close();
                    //if (tcpClient != null)
                    //{
                    //    tcpClient = null;
                    //}
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
        /// 关闭与服务器的连接
        /// </summary>
        /// <returns>异步TCP客户端</returns>
        public AsyncTcpClient Close()
        {
            if (Connected)
            {
                retries = 0;
                tcpClient.Close();
                tcpClient.Client.Disconnect(true);
                RaiseServerDisconnected(Addresses, Port);
            }
            return this;
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <returns>异步TCP客户端</returns>
        public AsyncTcpClient Connect()
        {
            try
            {
                if (!Connected)
                {
                    tcpClient.BeginConnect(Addresses, Port, HandleTcpServerConnected, tcpClient);
                }
            }
            catch (Exception)
            {

            }
            return this;
        }

        /// <summary>
        /// 连接服务器操作结束时回掉函数
        /// </summary>
        /// <param name="ar"></param>
        private void HandleTcpServerConnected(IAsyncResult ar)
        {
            try
            {
                tcpClient.EndConnect(ar);
                RaiseServerConnected(Addresses, Port);
                retries = 0;
            }
            catch (Exception ex)
            {
                retries++;
                if (retries > Retries)
                {
                    RaiseServerExceptionOccurred(Addresses, Port, ex);  //超过连接次数阈值,通知连接异常
                    return;
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(RetryInterval));
                    Connect();
                    return;
                }
            }

            byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
            tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, HandleDatagramReceived, buffer);
        }

        /// <summary>
        /// 接收服务器数据结束时回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void HandleDatagramReceived(IAsyncResult ar)
        {
            byte[] buffer = (byte[])ar.AsyncState;
            if (buffer != null && buffer.Length > 0)
            {
                foreach (byte b in buffer)
                {
                    receiveBuffer.Enqueue(b);
                }
            }
            tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, HandleDatagramReceived, buffer);
        }

        /// <summary>
        /// 解析接收的数据
        /// </summary>
        private void ParserReceiveDate()
        {
            Task.Factory.StartNew(() =>
            {
                while (receiveBuffer.Count >= 0)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                    if (receiveBuffer.Count <= 0) continue;

                    byte[] headbytes = new byte[2]; //报文头
                    if (receiveBuffer.Count < 2) return;
                    int headnum = 0;
                    while (true)
                    {
                        if (headnum >= headbytes.Length) break;
                        headbytes[headnum] = receiveBuffer.Dequeue();
                        headnum++;
                    }
                    //if () continue;

                    byte[] codebytes = new byte[2]; //功能码
                    if (receiveBuffer.Count < 2) return;
                    int codenum = 0;
                    while (true)
                    {
                        if (codenum >= codebytes.Length) break;
                        codebytes[codenum] = receiveBuffer.Dequeue();
                        codenum++;
                    }

                    byte[] lenthbytes = new byte[4]; //正文长度
                    if (receiveBuffer.Count < 4) return;
                    int lenthnum = 0;
                    while (true)
                    {
                        if (codenum >= lenthbytes.Length) break;
                        lenthbytes[lenthnum] = receiveBuffer.Dequeue();
                        lenthnum++;
                    }
                }
            });
        }


        ///// <summary>
        ///// 接收服务器数据结束时回调函数
        ///// </summary>
        ///// <param name="ar"></param>
        //private void HandleDatagramReceived(IAsyncResult ar)
        //{
        //    NetworkStream stream = tcpClient.GetStream();
        //    int numberOfReadBytes = 0;
        //    try
        //    {
        //        numberOfReadBytes = stream.EndRead(ar);
        //    }
        //    catch
        //    {
        //        numberOfReadBytes = 0;
        //    }
        //    if (numberOfReadBytes == 0)
        //    {
        //        Close();
        //        return;
        //    }
        //    byte[] buffer = (byte[])ar.AsyncState;
        //    byte[] receivedBytes = new byte[numberOfReadBytes];
        //    Buffer.BlockCopy(buffer, 0, receivedBytes, 0, numberOfReadBytes);
        //    var bytesList = dataParser.UnPackage(receivedBytes);
        //    if (bytesList != null && bytesList.Count > 0)
        //    {
        //        foreach (var item in bytesList)
        //        {
        //            RaiseDatagramReceived(tcpClient, item);
        //        }
        //    }
        //    stream.BeginRead(buffer, 0, buffer.Length, HandleDatagramReceived, buffer);
        //}


        /// <summary>
        /// 解析器返回数据事件
        /// </summary>
        /// <param name="obj"></param>
        private void DataParser_ReturnUnPackageBytes(byte[] obj)
        {
            //RaiseDatagramReceived(tcpClient, receivedBytes);
        }


        /// <summary>
        /// 发送报文
        /// </summary>
        /// <param name="datagram">报文</param>
        public void Send(byte[] datagram)
        {
            if (datagram == null)
                throw new ArgumentNullException("datagram");
            if (!Connected)
            {
                tcpClient.Client.Disconnect(true);
                RaiseServerDisconnected(Addresses, Port);
                throw new InvalidProgramException("This client has not connected to server.");
            }
            byte[] dataBytes = dataParser.StickPackage(datagram);  //粘包
            tcpClient.GetStream().BeginWrite(dataBytes, 0, dataBytes.Length, HandleDatagramWritten, tcpClient);
        }

        /// <summary>
        /// 往流中写入数据结束时回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void HandleDatagramWritten(IAsyncResult ar)
        {
            ((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);
        }
    }
}
