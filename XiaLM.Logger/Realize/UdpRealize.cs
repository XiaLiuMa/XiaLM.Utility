using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XiaLM.Logger.Help;
using XiaLM.Logger.Model;

namespace XiaLM.Logger.Realize
{
    public class UdpRealize
    {
        private Socket server;
        private List<EndPoint> clients;
        private CancellationTokenSource rToken;
        /// <summary>
        /// 新客户端插入事件
        /// </summary>
        public event Action<EndPoint> ClientInsertEvent = (p) => { };
        /// <summary>
        /// 新消息插入事件
        /// </summary>
        public event Action<EndPoint, InMsg> MessageInsertEvent = (p1, p2) => { };

        public UdpRealize()
        {
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "config.xml";
            if (!File.Exists(configPath)) throw new Exception("未找到配置文件！");
            Config config = XmlSerializeHelper.LoadXmlToObject<Config>(configPath);
            if (config == null) throw new Exception("配置文件格式不正确！");
            clients = new List<EndPoint>();
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Parse(config.UdpServer.Ip), config.UdpServer.Port));//绑定端口号和IP
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
            rToken?.Cancel();
        }

        /// <summary>  
        /// 接收发送给本机ip对应端口号的数据报  
        /// </summary>  
        public void ReciveMsg(CancellationToken token)
        {
            Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号  
                    byte[] buffer = new byte[2 * 1024]; //给2*1024个字节作为缓存
                    int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                    string message = Encoding.UTF8.GetString(buffer, 0, length);
                    InMsg imsg = JsonConvert.DeserializeObject<InMsg>(message);
                    if (imsg == null) break;
                    if (!clients.Contains(point))
                    {
                        clients.Add(point);
                        ClientInsertEvent(point);   //插入客户端
                    }
                    MessageInsertEvent(point, imsg);  //插入消息
                    if (imsg.Type.Equals("INFO"))
                    {
                        LogRealize.Info($"--[{imsg.Time}]{imsg.Message}",imsg.Client);
                    }
                    if (imsg.Type.Equals("WARN"))
                    {
                        LogRealize.Warn($"--[{imsg.Time}]{imsg.Message}", imsg.Client);
                    }
                    if (imsg.Type.Equals("ERROR"))
                    {
                        if (imsg.Exception == null)
                        {
                            LogRealize.Error($"--[{imsg.Time}]{imsg.Message}", imsg.Client);
                        }
                        else
                        {
                            LogRealize.Error(imsg.Exception, imsg.Client, $"--[{imsg.Time}]{imsg.Message}");
                        }
                    }
                    if (imsg.Type.Equals("FATAL"))
                    {
                        if (imsg.Exception == null)
                        {
                            LogRealize.Fatal($"--[{imsg.Time}]{imsg.Message}", imsg.Client);
                        }
                        else
                        {
                            LogRealize.Fatal(imsg.Exception, imsg.Client, $"--[{imsg.Time}]{imsg.Message}");
                        }
                    }
                    if (imsg.Type.Equals("DEBUG"))
                    {
                        if (imsg.Exception == null)
                        {
                            LogRealize.Debug($"--[{imsg.Time}]{imsg.Message}", imsg.Client);
                        }
                        else
                        {
                            LogRealize.Debug(imsg.Exception, imsg.Client, $"--[{imsg.Time}]{imsg.Message}");
                        }
                    }
                }
            }, token);
        }

        /// <summary>
        /// 移除指定客户端
        /// </summary>
        /// <param name="num"></param>
        public void RemoveClientAt(int num)
        {
            clients.RemoveAt(num);
        }

        /// <summary>
        /// 清除所有客户端
        /// </summary>
        public void ClearClient()
        {
            clients.Clear();
        }
    }

    /// <summary>
    /// 输入的消息
    /// </summary>
    public class InMsg
    {
        public string Client { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public string Time { get; set; }
    }
}
