using System;
using System.Windows.Forms;
using XiaLM.Log;

namespace XiaLM.FormTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //Application.Run(new MainForm());
            //Application.Run(new MicrosoftSpeech.MainForm());
            //Application.Run(new Serial.SerialForm());
            Application.Run(new Camera.CameraForm());
        }

        /// <summary>
        /// 异常未捕获时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string exception = sender.ToString() + e.ExceptionObject.ToString();
            MessageBox.Show(exception, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(-1);
        }
    }
}
