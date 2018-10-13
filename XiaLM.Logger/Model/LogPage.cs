using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelp.Model
{
    public class LogPage
    {
        /// <summary>
        /// 包括IP和端口(作为唯一标识)
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// 先入先出集合
        /// </summary>
        public Queue<LogMessage> QMesages { get; set; }
    }
}
