using System.Diagnostics;
using System.IO;

namespace XiaLM.Tool450.source.common
{
    /// <summary>
    /// 系统CMD后台操作助手
    /// </summary>
    public class CmdHelper
    {
        /// <summary>
        /// 隐藏启动即开即关CMD程序
        /// 【执行完命令后关闭CMD】
        /// </summary>
        /// <param name="cmd"></param>
        public static void RunCmdClose(string cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = @"cmd.exe";
            p.StartInfo.Arguments = @"/c " + cmd;
            p.StartInfo.UseShellExecute = false;    // 关闭Shell的使用
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            p.Close();
        }

        /// <summary>
        /// （旧版）隐藏启动即开即关CMD程序，已不用
        /// 【执行完命令后关闭CMD】
        /// </summary>
        /// <param name="cmd"></param>
        public static void RunCmdCloseOld(string cmd)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    // 关闭Shell的使用
            p.StartInfo.RedirectStandardInput = true;   // 重定向标准输入
            p.StartInfo.RedirectStandardOutput = true;  // 重定向标准输出
            p.StartInfo.RedirectStandardError = true;   //重定向错误输出 
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(cmd);
            p.StandardInput.AutoFlush = true;
            p.StandardInput.WriteLine("exit");
            StreamReader reader = p.StandardOutput;//截取输出流
            string output = reader.ReadLine();//每次读取一行
            while (!reader.EndOfStream)
            {
                output = reader.ReadLine();
            }
            p.WaitForExit();
        }
    }
}
