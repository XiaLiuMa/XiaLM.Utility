using System;
using System.Threading;
using XiaLM.Log;

namespace XiaLM.ConsoleTest.LogTest
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
                }
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            
        }
    }
}
