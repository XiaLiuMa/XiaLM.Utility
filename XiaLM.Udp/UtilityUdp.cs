using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XiaLM.Udp
{
    public class UtilityUdp
    {
        /// <summary>
        /// 发送超时
        /// </summary>
        public int SendTimeout { get; set; } = 1000;
        /// <summary>
        /// 接收超时
        /// </summary>
        public int ReceiveTimeout { get; set; } = 1000;
        public event Action<ReceiveData> ReceiveEvent = (s) => { };
        private CancellationTokenSource cts;
        UdpClient udp;
        IPEndPoint iPEndPoint;
        private int _hostPort;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hostPort">本地端口</param>
        public UtilityUdp(int hostPort)
        {
            Reset(hostPort);
        }
        /// <summary>
        ///  发送udp消息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="port">发送端口</param>
        public void SendUdp(byte[] content, int port)
        {
            try
            {
                Reset(_hostPort);
                udp.Send(content, content.Length, new IPEndPoint(IPAddress.Broadcast, port));
            }
            catch (Exception)
            {


            }
        }
        /// <summary>
        ///  发送udp消息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="port">发送端口</param>
        public void SendUdp(byte[] content, string ip, int port)
        {
            try
            {
                Reset(_hostPort);
                udp.Send(content, content.Length, new IPEndPoint(IPAddress.Parse(ip), port));
            }
            catch (Exception)
            {


            }
        }

        /// <summary>
        /// 接收监听
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public void Receive()
        {
            cts = new CancellationTokenSource();
            var token = cts.Token;
            var iep = new IPEndPoint(IPAddress.Any, 0);
            Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        byte[] recv = udp.Receive(ref iep);
                        ReceiveEvent(new ReceiveData()
                        {
                            Ip = iep.Address.ToString(),
                            Port = iep.Port,
                            Datas = recv
                        });
                    }
                    catch (Exception ex)
                    {


                    }
                }
            });
        }
        public void Reset(int hostPort)
        {
            _hostPort = hostPort;
            udp?.Close();
            iPEndPoint = new IPEndPoint(IPAddress.Any, hostPort);
            udp = new UdpClient(iPEndPoint);
            udp.Client.SendTimeout = SendTimeout;
            udp.Client.ReceiveTimeout = ReceiveTimeout;
        }

        public void Pause()
        {
            cts?.Cancel();
        }
        public void Stop()
        {
            Pause();
            udp.Close();
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        public class ReceiveData
        {
            /// <summary>
            /// 发送者ip
            /// </summary>
            public string Ip { get; internal set; }
            /// <summary>
            /// 发送者端口
            /// </summary>
            public int Port { get; internal set; }
            /// <summary>
            /// 数据
            /// </summary>
            public byte[] Datas { get; internal set; }
        }
    }
}
