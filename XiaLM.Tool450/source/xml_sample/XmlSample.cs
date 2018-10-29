using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tool450.source.xml_sample
{
    public class XmlSample
    {
        public void Test()
        {
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "config.xml";
            Config config = XmlSerializeHelper.LoadXmlToObject<Config>(configPath);
        }
    }
}
