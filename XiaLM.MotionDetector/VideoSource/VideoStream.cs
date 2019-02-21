using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaLM.MotionDetector.EventArg;

namespace XiaLM.MotionDetector.VideoSource
{
    public class VideoStream : IVideoSource
    {
        public string VideoSource { get; set; }
        public object UserData { get; set; }

        public int FramesReceived { get; set; }

        public int BytesReceived { get; set; }

        public bool Running { get; set; }

        public event CameraEventHandler NewFrame;

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
