using System;
using System.IO;

namespace XiaLM.Logger.Realize
{
    public class LogRealize
    {
        /// <summary>
        /// 输出INFO日志到LOG4NET
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="client"></param>
        public static void Info(string msg, string client)
        {
            log4net.LogManager.ResetConfiguration();
            log4net.GlobalContext.Properties["client"] = client;
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logCfg);
            log4net.ILog log = log4net.LogManager.GetLogger("INFO");
            log.Info(msg);
        }

        /// <summary>
        /// 输出Warn日志到Log4Net
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="client"></param>
        public static void Warn(string msg, string client)
        {
            log4net.GlobalContext.Properties["client"] = client;
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logCfg);
            log4net.ILog log = log4net.LogManager.GetLogger("WARN");
            log.Warn(msg);
        }

        /// <summary>
        /// 输出Error日志到Log4Net
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="client"></param>
        public static void Error(string msg,string client)
        {
            log4net.GlobalContext.Properties["client"] = client;
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logCfg);
            log4net.ILog log = log4net.LogManager.GetLogger("ERROR");
            log.Error(msg);
        }

        /// <summary>
        /// 输出Error日志到Log4Net
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public static void Error(Exception ex, string client, string msg = null)
        {
            log4net.GlobalContext.Properties["client"] = client;
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logCfg);
            log4net.ILog log = log4net.LogManager.GetLogger("ERROR");
            log.Error(msg, ex);
        }

        /// <summary>
        /// 输出Fatal日志到LOG4NET
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="client"></param>
        public static void Fatal(string msg, string client)
        {
            log4net.GlobalContext.Properties["client"] = client;
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logCfg);
            log4net.ILog log = log4net.LogManager.GetLogger("FATAL");
            log.Fatal(msg);
        }

        /// <summary>
        /// 输出Fatal日志到LOG4NET
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public static void Fatal(Exception ex, string client, string msg = null)
        {
            log4net.GlobalContext.Properties["client"] = client;
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logCfg);
            log4net.ILog log = log4net.LogManager.GetLogger("FATAL");
            log.Fatal(msg, ex);
        }

        /// <summary>
        /// 输出DEBUG日志到LOG4NET
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="client"></param>
        public static void Debug(string msg, string client)
        {
            log4net.GlobalContext.Properties["client"] = client;
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logCfg);
            log4net.ILog log = log4net.LogManager.GetLogger("DEBUG");
            log.Debug(msg);
        }

        /// <summary>
        /// 输出DEBUG日志到LOG4NET
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public static void Debug(Exception ex, string client, string msg = null)
        {
            log4net.GlobalContext.Properties["client"] = client;
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logCfg);
            log4net.ILog log = log4net.LogManager.GetLogger("DEBUG");
            log.Debug(msg, ex);
        }
    }
}
