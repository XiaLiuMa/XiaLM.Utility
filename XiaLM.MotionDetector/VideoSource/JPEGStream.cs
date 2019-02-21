using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XiaLM.MotionDetector.EventArg;

namespace XiaLM.MotionDetector.VideoSource
{
    public class JPEGStream : IVideoSource
    {
        private CancellationTokenSource cts;
        public event CameraEventHandler NewFrame;
        public bool Running { get; set; }

        private string source;
        private string login = null;
        private string password = null;
        private object userData = null;
        private int framesReceived;
        private int bytesReceived;
        private bool useSeparateConnectionGroup = false;
        private bool preventCaching = false;
        private int frameInterval = 0;      // frame interval in miliseconds

        private const int bufSize = 512 * 1024; // buffer size
        private const int readSize = 1024;      // portion size to read


        // SeparateConnectioGroup property
        // indicates to open WebRequest in separate connection group
        public bool SeparateConnectionGroup
        {
            get { return useSeparateConnectionGroup; }
            set { useSeparateConnectionGroup = value; }
        }
        // PreventCaching property
        // If the property is set to true, we are trying to prevent caching
        // appneding fake parameter to URL. It's needed is client is behind
        // proxy server.
        public bool PreventCaching
        {
            get { return preventCaching; }
            set { preventCaching = value; }
        }
        // FrameInterval property - interval between frames
        // If the property is set 100, than the source will produce 10 frames
        // per second if it possible
        public int FrameInterval
        {
            get { return frameInterval; }
            set { frameInterval = value; }
        }
        // VideoSource property
        public virtual string VideoSource
        {
            get { return source; }
            set { source = value; }
        }
        // Login property
        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        // Password property
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        // FramesReceived property
        public int FramesReceived
        {
            get
            {
                int frames = framesReceived;
                framesReceived = 0;
                return frames;
            }
        }
        // BytesReceived property
        public int BytesReceived
        {
            get
            {
                int bytes = bytesReceived;
                bytesReceived = 0;
                return bytes;
            }
        }
        // UserData property
        public object UserData
        {
            get { return userData; }
            set { userData = value; }
        }

        // Start work
        public void Start()
        {
            if (!this.Running)
            {
                framesReceived = 0;
                cts = new CancellationTokenSource();
                WorkerThread(cts.Token);
            }

        }

        // Abort thread
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
                byte[] buffer = new byte[bufSize];  // buffer to read stream
                HttpWebRequest req = null;
                WebResponse resp = null;
                Stream stream = null;
                Random rnd = new Random((int)DateTime.Now.Ticks);
                DateTime start;
                TimeSpan span;
                this.Running = true;
                while (!cts.IsCancellationRequested)
                {
                    int read, total = 0;
                    try
                    {
                        start = DateTime.Now;

                        // create request
                        if (!preventCaching)
                        {
                            req = (HttpWebRequest)WebRequest.Create(source);
                        }
                        else
                        {
                            req = (HttpWebRequest)WebRequest.Create(source + ((source.IndexOf('?') == -1) ? '?' : '&') + "fake=" + rnd.Next().ToString());
                        }
                        // set login and password
                        if ((login != null) && (password != null) && (login != ""))
                            req.Credentials = new NetworkCredential(login, password);
                        // set connection group name
                        if (useSeparateConnectionGroup)
                            req.ConnectionGroupName = GetHashCode().ToString();
                        // get response
                        resp = req.GetResponse();

                        // get response stream
                        stream = resp.GetResponseStream();

                        //// loop
                        //while (!stopEvent.WaitOne(0, true))
                        //{
                        //    // check total read
                        //    if (total > bufSize - readSize)
                        //    {
                        //        System.Diagnostics.Debug.WriteLine("flushing");
                        //        total = 0;
                        //    }

                        //    // read next portion from stream
                        //    if ((read = stream.Read(buffer, total, readSize)) == 0)
                        //        break;

                        //    total += read;

                        //    // increment received bytes counter
                        //    bytesReceived += read;
                        //}

                        //if (!stopEvent.WaitOne(0, true))
                        //{
                        //    // increment frames counter
                        //    framesReceived++;

                        //    // image at stop
                        //    if (NewFrame != null)
                        //    {
                        //        Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer, 0, total));
                        //        // notify client
                        //        NewFrame(this, new CameraEventArgs(bmp));
                        //        // release the image
                        //        bmp.Dispose();
                        //        bmp = null;
                        //    }
                        //}

                        //// wait for a while ?
                        //if (frameInterval > 0)
                        //{
                        //    // times span
                        //    span = DateTime.Now.Subtract(start);
                        //    // miliseconds to sleep
                        //    int msec = frameInterval - (int)span.TotalMilliseconds;

                        //    while ((msec > 0) && (stopEvent.WaitOne(0, true) == false))
                        //    {
                        //        // sleeping ...
                        //        Thread.Sleep((msec < 100) ? msec : 100);
                        //        msec -= 100;
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("=============: " + ex.Message);
                    }
                    finally
                    {
                        if (req != null)
                        {
                            req.Abort();
                            req = null;
                        }
                        if (stream != null)
                        {
                            stream.Close();
                            stream = null;
                        }
                        if (resp != null)
                        {
                            resp.Close();
                            resp = null;
                        }
                    }
                }
                this.Running = false;
            }, token);
        }
    }
}
