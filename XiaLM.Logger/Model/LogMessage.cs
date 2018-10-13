using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelp.Model
{
    /// <summary>
    /// 日志UI显示消息
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// 接收时间
        /// </summary>
        public string RTime { get; set; }
        /// <summary>
        /// 日志级别
        /// </summary>
        public string LogLevel { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; set; }
    }
}
