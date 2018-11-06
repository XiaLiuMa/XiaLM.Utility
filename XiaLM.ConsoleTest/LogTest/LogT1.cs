using System;
using System.IO;
using System.Threading;
using XiaLM.ConsoleTest.Model;
using XiaLM.Log;
using XiaLM.Tool450.source.common;

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

        public void Test1()
        {
            string strl = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory+ "AlgorithmClient_config.json");
            Rootobject obj = SerializeHelper.SerializeJsonToObject<Rootobject>(strl);
        }
    }
}
