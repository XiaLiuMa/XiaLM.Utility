using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace XiaLM.AForge.MotionDetection
{
    /// <summary>
    /// 运动识别器
    /// </summary>
    public class MotionRecognizer
    {
        private void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
            //nowImg = (Bitmap)image.Clone();
            //Bitmap objectImage = colorFilter.Apply(image);
            //BitmapData objectData = objectImage.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            //UnmanagedImage grayImage = grayFilter.Apply(new UnmanagedImage(objectData));
            //objectImage.UnlockBits(objectData);
            //blobCounter1.ProcessImage(grayImage);
            //List<Rectangle> rects = new List<Rectangle>();
            //rects.AddRange(blobCounter1.GetObjectsRectangles());

            //if (rects.Count > 0)
            //{
            //    for (int i = 0; i < rects.Count - 1; i++)
            //    {
            //        bool isNoTouchX = Math.Max(rects[i + 1].Right, rects[i].Right) - Math.Min(rects[i + 1].Left, rects[i].Left) > (rects[i].Width + rects[i + 1].Width);
            //        bool isNoTouchY = Math.Max(rects[i + 1].Bottom, rects[i].Top) - Math.Min(rects[i + 1].Top, rects[i].Top) > (rects[i].Height + rects[i + 1].Height);

            //    }
            //}


        }


    }
}
