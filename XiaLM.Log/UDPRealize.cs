using LogTool.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogTool
{
    public class UDPRealize
    {
        private Socket server { get; set; }  //UDP服务的Socket
        private Dictionary<string, string> targetDic;  //目标字典<ip,port>
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
            server.Bind(new IPEndPoint(IPAddress.Parse(GetLocalIP(true)), 6565));//绑定端口号和IP
            targetDic = new Dictionary<string, string>();
            UdpConfigInit();
        }

        /// <summary>
        /// UDP初始化设置
        /// </summary>
        private void UdpConfigInit()
        {
            var udpCfgFile = AppDomain.CurrentDomain.BaseDirectory + "UdpSendTarget.ini";
            if (!File.Exists(udpCfgFile))
            {
                var targetIp = GetLocalIP(true);
                using (StreamWriter sw = new StreamWriter(udpCfgFile, true))
                {
                    sw.Write(targetIp + ":6666");
                    sw.Flush();
                    sw.Close();
                }
            }
            string[] lines = File.ReadAllLines(udpCfgFile);
            foreach (string item in lines)
            {
                string[] texts = item.Split(':');
                targetDic.Add(texts[0], texts[1]);
            }
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
        /// <param name="msg"></param>
        public async void SendMsg(UDPMessage msgObj)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (KeyValuePair<string, string> item in targetDic)
                {
                    EndPoint point = new IPEndPoint(IPAddress.Parse(item.Key), Convert.ToInt32(item.Value));
                    server.SendTo(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msgObj)), point);
                }
            });
        }
    }
}
