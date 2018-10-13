using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelp.Model
{
    /// <summary>
    /// UDP消息实体
    /// </summary>
    public class UDPMessage
    {
        public string Message { get; set; }
        public LogLevel Level { get; set; }
        public Exception Exception { get; set; } 
    }

    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Error,
        Warn,
        Fatal
    }
}
