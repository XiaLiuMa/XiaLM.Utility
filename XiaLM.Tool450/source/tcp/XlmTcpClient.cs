//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using XlmUtility.source.common;

//namespace XiaLM.Tool450.source.tcp
//{
//    /// <summary>
//    /// Tcp客户端
//    /// </summary>
//    public class XlmTcpClient
//    {
//        private TcpClient tcpClient;
//        private string serverIp;    //服务端ip
//        private int serverPort; //服务端port
//        private int retries = 0;    //连接次数(计数用)

//        /// <summary>
//        /// 重连次数(默认为3次)
//        /// </summary>
//        public int conTimes { get; set; } = 3;
//        /// <summary>
//        /// 重连间隔时间(单位：秒，默认为3秒)
//        /// </summary>
//        public int conInterval { get; set; } = 3;
//        /// <summary>
//        /// 与服务器断开后是否自动连接(默认不自动连接)
//        /// </summary>
//        public bool autoConnect { get; set; }
//        /// <summary>
//        /// 数据解析器
//        /// </summary>
//        public IParser dataParser { get; set; }
//        /// <summary>
//        /// 是否已与服务器建立连接
//        /// </summary>
//        public bool Connected { get { return this.tcpClient.Client.Connected; } }

//        /// <summary>
//        /// 通知：连接服务端事件(包含连接成功，断开连接，连接异常)
//        /// </summary>
//        /// <param name="ipAddresses"></param>
//        /// <param name="port"></param>
//        private void NoticeConnectEvent(ConnectEventArgs args)
//        {
//            if (ConnectEventEvent != null) ConnectEventEvent(this, args);
//            if (autoConnect && args.Status.Equals(ConnectStatus.Disconnect))    //断开重连
//            {
//                Connect();
//            }
//        }
//        /// <summary>
//        /// 连接服务端成功事件
//        /// </summary>
//        public event EventHandler<ConnectEventArgs> ConnectEventEvent;

//        /// <summary>
//        /// 通知：接收到数据报文事件
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="datagram"></param>
//        private void NoticeClientReceivedEvent(ClientReceivedEventArgs args)
//        {
//            if (ClientReceivedEvent != null) ClientReceivedEvent(this, args);
//        }
//        /// <summary>
//        /// 接收到数据报文事件
//        /// </summary>
//        public event EventHandler<ClientReceivedEventArgs> ClientReceivedEvent;

//        public XlmTcpClient(string ip, int port) : this(ip, port, new XlmParser()) { }

//        /// <summary>
//        /// 异步TCP客户端
//        /// </summary>
//        /// <param name="ip"></param>
//        /// <param name="port"></param>
//        /// <param name="parser">用户可自定义解析器(未定义将实用默认解析器)</param>
//        public XlmTcpClient(string ip, int port, IParser parser)
//        {
//            this.serverIp = ip;
//            this.serverPort = port;
//            this.dataParser = parser;
//            this.tcpClient = new TcpClient();
//            dataParser.ReceiveDataEvent += DataParser_ReceiveDataEvent;
//        }

//                /// <summary>
//        /// 连接到服务器
//        /// </summary>
//        /// <returns>异步TCP客户端</returns>
//        public XlmTcpClient Connect()
//        {
//            try
//            {
//                if (!Connected)
//                {
//                    tcpClient.BeginConnect(this.serverIp, this.serverPort, ConnectedCallBack, tcpClient);
//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.WriteError(ex);
//            }
//            return this;
//        }

//        /// <summary>
//        /// 连接服务器操作结束时回调
//        /// </summary>
//        /// <param name="ar"></param>
//        private void ConnectedCallBack(IAsyncResult ar)
//        {
//            try
//            {
//                tcpClient.EndConnect(ar);
//                NoticeConnectEvent(new ConnectEventArgs(ConnectStatus.Connected, this.serverIp, this.serverPort));
//                retries = 0;    //置为0
//            }
//            catch (Exception ex)
//            {
//                retries++;
//                LogHelper.WriteError(ex);
//                if (retries > conTimes) //超过连接次数阈值,通知连接异常
//                {
//                    NoticeConnectEvent(new ConnectEventArgs(ConnectStatus.ConException, this.serverIp, this.serverPort, ex));
//                    return;
//                }
//                else
//                {
//                    Thread.Sleep(TimeSpan.FromSeconds(conInterval));
//                    Connect();
//                    return;
//                }
//            }
//            byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
//            tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, ReceivedCallBack, buffer);
//        }

//        /// <summary>
//        /// 单次接收完数据时回调
//        /// </summary>
//        /// <param name="ar"></param>
//        private void ReceivedCallBack(IAsyncResult ar)
//        {
//            byte[] buffer = (byte[])ar.AsyncState;
//            dataParser.Decoding(buffer,(p)=> { });
//            tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, ReceivedCallBack, buffer);
//        }

//        private void DataParser_ReceiveDataEvent(byte[] obj)
//        {
//            NoticeClientReceivedEvent(new ClientReceivedEventArgs(this.tcpClient, obj));
//        }


//        /// <summary>
//        /// 发送报文
//        /// </summary>
//        /// <param name="datagram">报文</param>
//        public void Send(byte[] data)
//        {
//            if (data == null) throw new ArgumentNullException("data is null");
//            if (!Connected)
//            {
//                tcpClient.Client.Disconnect(true);
//                NoticeConnectEvent(new ConnectEventArgs(ConnectStatus.Disconnect, this.serverIp, this.serverPort));
//                throw new InvalidProgramException("This client has not connected to server.");
//            }
//            byte[] dataBytes = dataParser.Encoding(data);  //粘包
//            tcpClient.GetStream().BeginWrite(dataBytes, 0, dataBytes.Length, WrittenCallBack, tcpClient);
//        }

//        /// <summary>
//        /// 往流中写入数据结束时回调
//        /// </summary>
//        /// <param name="ar"></param>
//        private void WrittenCallBack(IAsyncResult ar)
//        {
//            ((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);
//        }

//        /// <summary>
//        /// 销毁客户端
//        /// </summary>
//        public void DisposeCilent()
//        {
//            try
//            {
//                if (Connected)
//                {
//                    retries = 0;
//                    tcpClient.Close();
//                    tcpClient.Client.Disconnect(true);
//                    this.autoConnect = false;   //关闭自动连接
//                    NoticeConnectEvent(new ConnectEventArgs(ConnectStatus.Disconnect, this.serverIp, this.serverPort));
//                }
//                GC.SuppressFinalize(this);
//            }
//            catch (SocketException ex)
//            {
//                LogHelper.WriteError(ex);
//            }
//        }
//    }

//    /// <summary>
//    /// 连接服务器事件参数(包含连接成功，断开连接，连接异常)
//    /// </summary>
//    public class ConnectEventArgs : EventArgs
//    {
//        /// <summary>
//        /// 连接状态
//        /// </summary>
//        public ConnectStatus Status { get; private set; }
//        /// <summary>
//        /// 服务器IP地址
//        /// </summary>
//        public string Ip { get; private set; }
//        /// <summary>
//        /// 服务器端口
//        /// </summary>
//        public int Port { get; private set; }
//        /// <summary>
//        /// 内部异常
//        /// </summary>
//        public Exception Ex { get; private set; }

//        public ConnectEventArgs(ConnectStatus status, string ip, int port) : this(status, ip, port, null) { }

//        public ConnectEventArgs(ConnectStatus status, string ip, int port, Exception ex)
//        {
//            this.Status = status;
//            this.Ip = ip;
//            this.Port = port;
//            this.Ex = ex;
//        }
//    }

//    /// <summary>
//    /// 客户端接收到数据报文事件参数
//    /// </summary>
//    /// <typeparam name="T">报文类型</typeparam>
//    public class ClientReceivedEventArgs : EventArgs
//    {
//        /// <summary>
//        /// 客户端
//        /// </summary>
//        public TcpClient TcpClient { get; private set; }
//        /// <summary>
//        /// 报文
//        /// </summary>
//        public byte[] Data { get; private set; }

//        /// <summary>
//        /// 接收到数据报文事件参数
//        /// </summary>
//        /// <param name="tcpClient">客户端</param>
//        /// <param name="datagram">报文</param>
//        public ClientReceivedEventArgs(TcpClient tcpClient, byte[] data)
//        {
//            TcpClient = tcpClient;
//            Data = data;
//        }
//    }

//    /// <summary>
//    /// 连接状态
//    /// </summary>
//    public enum ConnectStatus
//    {
//        /// <summary>
//        /// 已连接
//        /// </summary>
//        Connected,
//        /// <summary>
//        /// 断开连接
//        /// </summary>
//        Disconnect,
//        /// <summary>
//        /// 连接异常
//        /// </summary>
//        ConException
//    }

//}
