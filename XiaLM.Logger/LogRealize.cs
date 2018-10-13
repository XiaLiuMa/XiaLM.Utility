using log4net;
using log4net.Config;
using LogHelp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogHelp.Model.UDPMessage;

namespace LogHelp
{
    public class LogRealize
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILog _log;
        private static readonly object lockObj = new object();
        private static LogRealize logRealize;
        public static LogRealize GetInstance()
        {
            if (logRealize == null)
            {
                lock (lockObj)
                {
                    if (logRealize == null)
                    {
                        logRealize = new LogRealize();
                    }
                }
            }
            return logRealize;
        }
        public LogRealize()
        {
            var configFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
            if (!configFile.Exists)
            {
                throw new Exception("未配置log4net配置文件！");
            }
            // 设置日志配置文件路径
            XmlConfigurator.Configure(configFile);
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="uMessage"></param>
        public void WriteLog(UDPMessage uMessage)
        {
            if (_log.IsDebugEnabled)
            {
                switch (uMessage.Level)
                {
                    case LogLevel.Debug:
                        _log.Debug(uMessage.Message, uMessage.Exception);
                        break;
                    case LogLevel.Info:
                        _log.Info(uMessage.Message, uMessage.Exception);
                        break;
                    case LogLevel.Error:
                        _log.Error(uMessage.Message, uMessage.Exception);
                        break;
                    case LogLevel.Warn:
                        _log.Warn(uMessage.Message, uMessage.Exception);
                        break;
                    case LogLevel.Fatal:
                        _log.Fatal(uMessage.Message, uMessage.Exception);
                        break;
                }
            }
        }
    }
}
