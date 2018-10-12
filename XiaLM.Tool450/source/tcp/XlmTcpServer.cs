//using System;
//using System.Net.Sockets;

//namespace XiaLM.Tool450.source.tcp
//{
//    public class XlmTcpServer
//    {
//        private TcpListener tcpListener;

//        /// <summary>
//        /// 服务器是否正在运行
//        /// </summary>
//        public bool isRunning { get; private set; }




//        /// <summary>
//        /// 启动Tcp服务器
//        /// </summary>
//        /// <param name="backlog">服务器所允许的挂起连接序列的最大长度/// </param>
//        /// <returns>异步TCP服务器</returns>
//        public void Start(int backlog = 0)
//        {
//            if (!isRunning)
//            {
//                if (backlog != 0)
//                {
//                    tcpListener.Start(backlog);
//                }
//                else
//                {
//                    tcpListener.Start();
//                }
//                //监听新客户端
//                tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnected), tcpListener);
//                isRunning = true;
//            }
//        }

//        /// <summary>
//        /// 接收到新的客户端连接消息时回调函数
//        /// </summary>
//        /// <param name="ar"></param>
//        private void HandleClientConnected(IAsyncResult ar)
//        {
//            if (IsRunning)
//            {
//                TcpListener tcpListener = (TcpListener)ar.AsyncState;
//                TcpClient tcpClient = tcpListener.EndAcceptTcpClient(ar);
//                TcpClientEntity clientEntity = new TcpClientEntity(tcpClient);
//                lock (this.clientDictionary)
//                {
//                    string clientId = GetRemoteIpEndPoint(tcpClient);
//                    this.clientDictionary.Add(clientId, tcpClient);
//                    NoticeClientConnected(tcpClient);
//                }
//                clientEntity.BeginReadCilentMsg();   //开始监听客户端
//                clientEntity.ReturnColdPackageEvent += new Action<byte[]>((p) =>    //接收到客户端完整消息事件
//                {
//                    NoticeDatagramReceived(clientEntity.TcpClient, p);
//                });
//                clientEntity.ClientDisconnectedEvent += new Action(() =>    //接收到客户端掉线事件
//                {
//                    lock (this.clientDictionary)
//                    {
//                        string clientId = GetRemoteIpEndPoint(clientEntity.TcpClient);
//                        clientEntity.TcpClient.Client.Disconnect(true);
//                        this.clientDictionary.Remove(clientId);
//                        NoticeClientDisconnected(clientEntity.TcpClient);
//                    }
//                });
//                tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnected), ar.AsyncState);//继续监听新客户端
//            }
//        }


//    }
//}
