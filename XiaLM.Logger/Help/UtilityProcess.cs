using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Logger.Help
{
    /// <summary>
    /// 程序启动帮助类
    /// </summary>
    public class UtilityProcess
    {
        private static string _workingDirectory;//程序工作目录 
        /// <summary>
        /// 程序工作目录
        /// </summary>
        public static string WorkingDirectory
        {
            get { return _workingDirectory; }
            set { _workingDirectory = value; }
        }
        public static void Start(string executablePath, bool isHidden = false, bool _isAdmin = false)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = _workingDirectory;
            startInfo.FileName = executablePath;
            if (isHidden)
            {
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            //设置启动动作,确保以管理员身份运行
            if (_isAdmin)
            {
                startInfo.Verb = "runas";
            }
            try
            {
                System.Diagnostics.Process.Start(startInfo);
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 杀死进程
        /// </summary>
        /// <param name="processName"></param>
        public static void Kill(string processName)
        {
            var list = System.Diagnostics.Process.GetProcesses().Where(p => p.ProcessName.Equals(processName)).ToList();
            list.ForEach(r => r.Kill());
        }
        /// <summary>
        /// 当前程序是否运行
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool GetPorcessState(string name)
        {
            System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcesses();
            System.Diagnostics.Process myProcess = myProcesses.Where(r => r.ProcessName.Equals(name)).FirstOrDefault();
            if (myProcess == null)
                return false;
            else
                return true;
        }

    }
}
