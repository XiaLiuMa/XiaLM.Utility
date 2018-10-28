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
            LogOutPut.GetInstance().Init("127.0.0.1", 51888);
            //LogOutPut.GetInstance().Init("192.168.2.101", 51888);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            //Application.Run(new MicrosoftSpeech.MainForm());
        }
    }
}
