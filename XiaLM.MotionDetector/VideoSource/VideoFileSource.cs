using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XiaLM.Log;
using XiaLM.MotionDetector.Comon;
using XiaLM.MotionDetector.EventArg;

namespace XiaLM.MotionDetector.VideoSource
{
    public class VideoFileSource : IVideoSource
    {
        private CancellationTokenSource cts;
        public string VideoSource { get; set; }
        public bool Running { get; set; }
        public object UserData { get; set; }
        public int FramesReceived { get; set; }
        public int BytesReceived { get; set; }
        public event CameraEventHandler NewFrame;

        public void Start()
        {
            if (!this.Running)
            {
                FramesReceived = 0;
                cts = new CancellationTokenSource();
                WorkerThread(cts.Token);
            }
        }

        public void Stop()
        {
            if (this.Running)
            {
                cts?.Cancel();
            }
        }

        private void WorkerThread(CancellationToken token)
        {
            Task.Factory.StartNew(() =>
            {
                this.Running = true;
                AVIReader aviReader = new AVIReader();
                try
                {
                    aviReader.Open(VideoSource); // open file
                    while (!cts.IsCancellationRequested)
                    {
                        Bitmap bmp = aviReader.GetNextFrame();  // get next frame
                        FramesReceived++;
                        if (NewFrame != null) NewFrame(this, new CameraEventArgs(bmp));
                        bmp.Dispose();  // free image
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
                aviReader.Dispose();
                aviReader = null;
                this.Running = false;
            }, token);
        }
    }
}
