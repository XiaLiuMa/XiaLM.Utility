using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Logger.Model
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 地图设置
        /// </summary>
        public UdpServer UdpServer { get; set; }
    }
    /// <summary>
    /// UDP服务端
    /// </summary>
    public class UdpServer
    {
        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }
    }
}
