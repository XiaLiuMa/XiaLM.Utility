using log4net;
using log4net.Config;
using XiaLM.Log.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Log
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        private readonly ILog _log;
        private static readonly object lockObj = new object();
        private static LogHelper logHelper;
        public static LogHelper GetInstance()
        {
            if (logHelper == null)
            {
                lock (lockObj)
                {
                    if (logHelper == null)
                    {
                        logHelper = new LogHelper();
                    }
                }
            }
            return logHelper;
        }
        public LogHelper()
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
        /// Debug调试
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="ex">异常</param>
        public void Debug(string msg, Exception ex = null)
        {
            _log.Debug(msg, ex);
            UDPRealize.GetInstance().SendMsg(new UDPMessage()
            {
                Message = msg,
                Level = LogLevel.Debug,
                Exception = ex
            });
        }
        /// <summary>
        /// 正常信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="ex">异常</param>
        public void Info(string msg, Exception ex = null)
        {
            _log.Info(msg, ex);
            UDPRealize.GetInstance().SendMsg(new UDPMessage()
            {
                Message = msg,
                Level = LogLevel.Info,
                Exception = ex
            });
        }
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="ex">异常</param>
        public void Warn(string msg, Exception ex = null)
        {
            _log.Warn(msg, ex);
            UDPRealize.GetInstance().SendMsg(new UDPMessage()
            {
                Message = msg,
                Level = LogLevel.Warn,
                Exception = ex
            });
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="ex">异常</param>
        public void Error(string msg, Exception ex = null)
        {
            _log.Error(msg, ex);
            UDPRealize.GetInstance().SendMsg(new UDPMessage()
            {
                Message = msg,
                Level = LogLevel.Error,
                Exception = ex
            });
        }
        /// <summary>
        /// 致命错误信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="ex">异常</param>
        /// <param name="isSendOut">是否对外发送</param>
        public void Fatal(string msg, Exception ex = null)
        {
            _log.Fatal(msg, ex);
            UDPRealize.GetInstance().SendMsg(new UDPMessage()
            {
                Message = msg,
                Level = LogLevel.Fatal,
                Exception = ex
            });
        }
    }
}
