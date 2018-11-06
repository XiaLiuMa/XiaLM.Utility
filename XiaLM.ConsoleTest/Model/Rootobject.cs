using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.ConsoleTest.Model
{

    public class Rootobject
    {
        public string AlgorithmIp { get; set; }
        public int AlgorithmPort { get; set; }
        public string FaceContrastEndpoint { get; set; }
        public string CameraIp { get; set; }
        public int CameraPort { get; set; }
        public string CameraEndpoint { get; set; }
        public ushort FilteringArea { get; set; }
    }

}
