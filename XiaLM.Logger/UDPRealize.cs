using XiaLM.Logger.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static XiaLM.Logger.Model.UDPMessage;

namespace XiaLM.Logger
{
    public class UDPRealize
    {
        public List<LogPage> logPages { get; set; } = new List<LogPage>();
        private CancellationTokenSource rToken;
        private Socket server { get; set; }  //UDP服务的Socket
        public event Action LogPageQueueEvent = () => { };  //日志页入队列事件
        public event Action<LogPage> MesageQueueEvent = (p) => { };   //信息入队列事件
        private static readonly object lockObj = new object();
        private static UDPRealize uDPRealize;
        public static UDPRealize GetInstance()
        {
            if (uDPRealize == null)
            {
                lock (lockObj)
                {
                    if (uDPRealize == null)
                    {
                        uDPRealize = new UDPRealize();
                    }
                }
            }
            return uDPRealize;
        }
        public UDPRealize()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Parse(GetLocalIP(true)), 6666));//绑定端口号和IP
        }

        /// <summary>
        /// 获取本机IPv4地址(以太网/无线网2中)
        /// </summary>
        /// <param name="isWlan">是否是获取无线网的ip，否则获取的是以太网的ip</param>
        /// <param name="num">选取第几个ip，同一个网卡可能开了几个网络，默认不传为第一个</param>
        /// <returns></returns>
        private string GetLocalIP(bool isWlan, int num = 0)
        {
            List<NetworkInterface> interfaceList = new List<NetworkInterface>();
            try
            {
                List<NetworkInterface> interfaces = NetworkInterface.GetAllNetworkInterfaces().ToList();
                if (isWlan) //无线网
                {
                    interfaceList = interfaces.Where(p => p.NetworkInterfaceType.Equals(NetworkInterfaceType.Wireless80211)).ToList();
                }
                else //以太网
                {
                    interfaceList = interfaces.Where(p => p.NetworkInterfaceType.Equals(NetworkInterfaceType.Ethernet)).ToList();
                }
                if (interfaceList != null && interfaceList.Count > 0)
                {
                    List<string> ips = new List<string>();
                    foreach (NetworkInterface item in interfaceList)
                    {
                        var property = item.GetIPProperties();
                        if (property.GatewayAddresses.Count <= 0) continue;
                        if (property.UnicastAddresses != null && property.UnicastAddresses.Count > 0)
                        {
                            foreach (UnicastIPAddressInformation ip in property.UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    ips.Add(ip.Address.ToString());
                                }
                            }
                        }
                    }
                    if (ips.Count > 0) return ips[num];
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
                throw ex;
            }
        }

        /// <summary>  
        /// 向特定ip的主机的端口发送数据报  
        /// </summary>  
        public async Task SendMsg()
        {
            await Task.Factory.StartNew(() =>
            {
                EndPoint point = new IPEndPoint(IPAddress.Parse("169.254.202.67"), 6000);
                while (true)
                {
                    string msg = Console.ReadLine();
                    server.SendTo(Encoding.UTF8.GetBytes(msg), point);
                }
            });
        }

        /// <summary>
        /// 开始接收UDP信息
        /// </summary>
        public void StartRecive()
        {
            rToken = new CancellationTokenSource();
            ReciveMsg(rToken.Token);
        }

        /// <summary>
        /// 停止接收UDP信息
        /// </summary>
        public void StopRecive()
        {
            rToken.Cancel();
            logPages.Clear();
        }

        /// <summary>  
        /// 接收发送给本机ip对应端口号的数据报  
        /// </summary>  
        public void ReciveMsg(CancellationToken token)
        {
            Task.Factory.StartNew(() =>
            {
                int cNumber = 0;  //客户端个数
                while (!token.IsCancellationRequested)
                {
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号  
                    byte[] buffer = new byte[1024];
                    int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                    string message = Encoding.UTF8.GetString(buffer, 0, length);
                    string reciveTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");   //接收时间
                    UDPMessage msgObj = JsonConvert.DeserializeObject<UDPMessage>(message);
                    LogRealize.GetInstance().WriteLog(msgObj);  //写日志
                    string tempLevel = string.Empty;    //日志级别
                    switch (msgObj.Level)
                    {
                        case LogLevel.Debug:
                            tempLevel = "Debug";
                            break;
                        case LogLevel.Info:
                            tempLevel = "Info";
                            break;
                        case LogLevel.Error:
                            tempLevel = "Error";
                            break;
                        case LogLevel.Warn:
                            tempLevel = "Warn";
                            break;
                        case LogLevel.Fatal:
                            tempLevel = "Fatal";
                            break;
                    }
                    LogMessage uiMsgObj = new LogMessage()
                    {
                        RTime = reciveTime,
                        LogLevel = tempLevel,
                        Message = msgObj.Message,
                        Exception = msgObj.Exception
                    };
                    var cPoint = point.ToString();
                    if (logPages.Count > 0)
                    {
                        var logPageObj = logPages.Find(p => p.Point.Equals(cPoint));
                        if (logPageObj != null)
                        {
                            if (logPageObj.QMesages.Count > 1000)
                            {
                                logPages.Find(p => p.Point.Equals(cPoint)).QMesages.Dequeue();
                            }
                            logPages.Find(p => p.Point.Equals(cPoint)).QMesages.Enqueue(uiMsgObj);
                            MesageQueueEvent(logPages.Find(p => p.Point.Equals(cPoint)));
                        }
                        else
                        {
                            cNumber += 1;
                            logPages.Add(new LogPage()
                            {
                                Point = cPoint,
                                QMesages = new Queue<LogMessage>()
                            });
                            logPages.Find(p => p.Point.Equals(cPoint)).QMesages.Enqueue(uiMsgObj);
                            LogPageQueueEvent();
                        }
                    }
                    else
                    {
                        logPages.Add(new LogPage()
                        {
                            Point = cPoint,
                            QMesages = new Queue<LogMessage>()
                        });
                        logPages.Find(p => p.Point.Equals(cPoint)).QMesages.Enqueue(uiMsgObj);
                        LogPageQueueEvent();
                    }
                }
            }, token);
        }
    }
}
