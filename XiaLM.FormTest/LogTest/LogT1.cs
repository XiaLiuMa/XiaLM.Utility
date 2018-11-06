using System;
using System.Threading;
using XiaLM.Log;

namespace XiaLM.FormTest.LogTest
{
    public class LogT1
    {
        private byte[] bytes;
        public void Test()
        {
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    foreach (var item in bytes)
                    {
                        byte b = item;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    byte code = 0x01;
                    byte[] body = new byte[8] { 0x01,0x01, 0x01, 0x06, 0x01, 0x01, 0x08, 0x01};

                    string temStr = string.Empty;
                    for (int j = 0; j < body.Length; j++)
                    {
                        temStr += body[j].ToString("X2");
                        if (j >= 5)
                        {
                            temStr += "...";
                            break;
                        }
                    }
                    Logger.Info($"接收算法数据，功能码[{code.ToString("X2")}],正文[{temStr}]！");
                }
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            
        }
    }
}
