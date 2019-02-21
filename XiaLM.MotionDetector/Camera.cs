using System;
using System.Drawing;
using System.Threading;
using XiaLM.Log;
using XiaLM.MotionDetector.MotionDetector;
using XiaLM.MotionDetector.VideoSource;

namespace XiaLM.MotionDetector
{
    public class Camera
    {
        private Bitmap _LastBitmap = null;
        public Bitmap LastFrame
        { get { return _LastBitmap; } }
        public int Width { get; private set; } = -1;
        public int Height { get; private set; } = -1;
        public IVideoSource VideoSource { get; private set; } = null;
        public IMotionDetector MotionDetecotor { get; private set; } = null;
        public int FramesReceived
        {
            get { return (VideoSource == null) ? 0 : VideoSource.FramesReceived; }
        }
        // BytesReceived property
        public int BytesReceived
        {
            get { return (VideoSource == null) ? 0 : VideoSource.BytesReceived; }
        }
        // Running property
        public bool Running
        {
            get { return (VideoSource == null) ? false : VideoSource.Running; }
        }
        private double alarmLevel = 0.005;
        public event EventHandler NewFrame;
        public event EventHandler Alarm;

        public Camera(IVideoSource source, IMotionDetector detector)
        {
            this.VideoSource = source;
            this.MotionDetecotor = detector;
            VideoSource.NewFrame += VideoSource_NewFrame;
        }

        public void Start()
        {
            if (VideoSource != null)
            {
                VideoSource.Start();
            }
        }

        // Abort camera
        public void Stop()
        {
            Monitor.Enter(this);    // lock
            if (VideoSource != null)
            {
                VideoSource.Stop();
            }
            Monitor.Exit(this); // unlock
        }

        // Lock it
        public void Lock()
        {
            Monitor.Enter(this);
        }

        // Unlock it
        public void Unlock()
        {
            Monitor.Exit(this);
        }

        private void VideoSource_NewFrame(object sender, EventArg.CameraEventArgs e)
        {
            try
            {
                // lock
                Monitor.Enter(this);

                // dispose old frame
                if (_LastBitmap != null) _LastBitmap.Dispose();
                _LastBitmap = (Bitmap)e.Bitmap.Clone();

                // apply motion detector
                if (MotionDetecotor != null)
                {
                    MotionDetecotor.ProcessFrame(ref _LastBitmap);

                    // check motion level
                    if (
                        (MotionDetecotor.MotionLevel >= alarmLevel) &&
                        (Alarm != null)
                        )
                    {
                        Alarm(this, new EventArgs());
                    }
                }
                Width = _LastBitmap.Width;
                Height = _LastBitmap.Height;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                Monitor.Exit(this); // unlock
            }

            if (NewFrame != null) NewFrame(this, new EventArgs());  // notify client

        }
    }
}
