using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XiaLM.Tool450.source.common
{
    /// <summary>
    /// 进程助手
    /// </summary>
    public class ProcessHelper
    {
        /// <summary>
        /// 通过进程Id获取进程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Process GetProcessById(int id)
        {
            Process[] currentprocess = Process.GetProcesses();  //获取系统内所有进程
            foreach (Process p in currentprocess)
            {
                if (p.Id.Equals(id)) return p;
            }
            return null;
        }

        /// <summary>
        /// 通过进程Name获取进程
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Process GetProcessByName(string name)
        {
            Process[] currentprocess = Process.GetProcesses();  //获取系统内所有进程
            foreach (Process p in currentprocess)
            {
                if (p.ProcessName.Equals(name)) return p;
            }
            return null;
        }

        /// <summary>
        /// 通过进程Id关闭进程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool KillProcessById(int id)
        {
            Process process = null;
            Process[] currentprocess = Process.GetProcesses();  //获取系统内所有进程
            foreach (Process p in currentprocess)
            {
                if (p.Id.Equals(id)) process = p;
            }
            if (process == null) return false;
            return KillProcess(process);
        }

        /// <summary>
        /// 通过进程Name关闭进程
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool KillProcessByName(string name)
        {
            Process process = null;
            Process[] currentprocess = Process.GetProcesses();  //获取系统内所有进程
            foreach (Process p in currentprocess)
            {
                if (p.ProcessName.Equals(name)) process = p;
            }
            if (process == null) return false;
            return KillProcess(process);
        }

        /// <summary>
        /// 关闭进程
        /// 【(每过1秒中)检查进程是否杀死，连续3次未杀死便返回关闭进程失败】
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static bool KillProcess(Process process)
        {
            bool flag = false;
            process.Kill();
            for (int i = 0; i < 3; i++)
            {
                Process[] currentprocess = Process.GetProcesses();  //获取系统内所有进程
                if (currentprocess.Contains(process))   //判断是否包含
                {
                    process.Kill();
                }
                else
                {
                    flag = true;
                    break;
                }
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            return flag;
        }
    }
}
