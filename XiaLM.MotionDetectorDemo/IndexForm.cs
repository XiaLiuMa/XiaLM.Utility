using System;
using System.Windows.Forms;
using XiaLM.MotionDetector;
using XiaLM.MotionDetector.Comon;
using XiaLM.MotionDetector.MotionDetector;
using XiaLM.MotionDetector.VideoSource;
using XiaLM.MotionDetectorDemo.UI.UControl;

namespace XiaLM.MotionDetectorDemo
{
    public partial class IndexForm : Form
    {
        private int intervalsToSave = 0;
        private System.Timers.Timer timer;
        private bool saveOnMotion = false;
        private AVIWriter writer = null;
        private int statIndex = 0, statReady = 0;

        private IMotionDetector _MotionDetector;
        private CameraWindow _CameraWindow;
        public IndexForm()
        {
            InitializeComponent();
            _MotionDetector = new MotionDetectorA();
        }

        private void IndexForm_Load(object sender, EventArgs e)
        {
            this.OpenGifItem.Click += OpenGifItem_Click;
            this.OpenVideoItem.Click += OpenVideoItem_Click;
            this.DetectorItem1.Click += DetectorItems_Click;
        }

        private void DetectorItems_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenGifItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Gif files (*.gif)|*.gig",
                Title = "Open Gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                VideoFileSource fileSource = new VideoFileSource()
                {
                    VideoSource = openFileDialog.FileName
                };
                OpenVideoSource(fileSource);
            }
        }

        private void OpenVideoItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "AVI files (*.avi)|*.avi",
                Title = "Open movie"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                VideoFileSource fileSource = new VideoFileSource()
                {
                    VideoSource = openFileDialog.FileName
                };
                OpenVideoSource(fileSource);
            }
        }

        private void OpenVideoSource(IVideoSource source)
        {
            this.Cursor = Cursors.WaitCursor;   // set busy cursor
            CloseFile();    // close previous file

            // enable/disable motion alarm
            if (_MotionDetector != null)
            {
                _MotionDetector.MotionLevelCalculation = DetectorItem.Checked;
            }

            Camera camera = new Camera(source, _MotionDetector);
            camera.NewFrame += Camera_NewFrame;
            camera.Alarm += Camera_Alarm;
            camera.Start();
            _CameraWindow.Camera = camera;  // attach camera to camera window
            statIndex = statReady = 0;  // reset statistics
            timer.Start();  // start timer
            this.Cursor = Cursors.Default;
        }

        private void Camera_NewFrame(object sender, EventArgs e)
        {
            if ((intervalsToSave != 0) && (saveOnMotion == true))
            {
                // lets save the frame
                if (writer == null)
                {
                    string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".avi";
                    try
                    {
                        writer = new AVIWriter("wmv3"); // create AVI writer
                        writer.Open(fileName, _CameraWindow.Camera.Width, _CameraWindow.Camera.Height); // open AVI file
                    }
                    catch (ApplicationException ex)
                    {
                        if (writer != null)
                        {
                            writer.Dispose();
                            writer = null;
                        }
                    }
                }

                Camera camera = _CameraWindow.Camera;   // save the frame
                camera.Lock();
                writer.AddFrame(camera.LastFrame);
                camera.Unlock();
            }
        }

        private void Camera_Alarm(object sender, EventArgs e)
        {
            intervalsToSave = (int)(5 * (1000 / timer.Interval));
        }

        private void CloseFile()
        {
            Camera camera = _CameraWindow.Camera;
            if (camera != null)
            {
                _CameraWindow.Camera = null;    // detach camera from camera window
                camera.Stop();
                camera = null;

                if (_MotionDetector != null) _MotionDetector.Reset();
            }

            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
            intervalsToSave = 0;
        }
    }
}
