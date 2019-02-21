using System;

namespace XiaLM.MotionDetector.EventArg
{
    public delegate void CameraEventHandler(object sender, CameraEventArgs e);
    public class CameraEventArgs : EventArgs
    {
        private System.Drawing.Bitmap bmp;

        // Constructor
        public CameraEventArgs(System.Drawing.Bitmap bmp)
        {
            this.bmp = bmp;
        }

        // Bitmap property
        public System.Drawing.Bitmap Bitmap
        {
            get { return bmp; }
        }
    }
}
