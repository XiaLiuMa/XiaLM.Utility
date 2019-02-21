using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaLM.AForge;

namespace XiaLM.ConsoleTest.AForge
{
    public class ColorTest
    {
        public void SaveAllColorToBmp(string filePath)
        {
            ColorManager colorManager = new ColorManager();
            colorManager.SaveAllColorToBmp(filePath);
        }
    }
}
