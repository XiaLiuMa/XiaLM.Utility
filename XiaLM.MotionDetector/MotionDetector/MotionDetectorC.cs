﻿using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing;
using System.Drawing.Imaging;
using XiaLM.Log;

namespace XiaLM.MotionDetector.MotionDetector
{
    public class MotionDetectorC : IMotionDetector
    {
        private int counter = 0;
        private Bitmap backgroundFrame;
        private BitmapData bitmapData;

        private IFilter grayscaleFilter = new GrayscaleBT709();
        private Difference differenceFilter = new Difference();
        private Threshold thresholdFilter = new Threshold(15);
        private IFilter openingFilter = new Opening();
        private IFilter edgesFilter = new Edges();
        private Merge mergeFilter = new Merge();

        private IFilter extrachChannel = new ExtractChannel(RGB.R);
        private ReplaceChannel replaceChannel = new ReplaceChannel(RGB.R, (Bitmap)null);
        private MoveTowards moveTowardsFilter = new MoveTowards();

        private bool calculateMotionLevel = false;
        private int width;  // image width
        private int height; // image height
        private int pixelsChanged;

        public bool MotionLevelCalculation
        {
            get { return calculateMotionLevel; }
            set { calculateMotionLevel = value; }
        }

        public double MotionLevel
        {
            get { return (double)pixelsChanged / (width * height); }
        }

        public void Reset()
        {
            if (backgroundFrame != null)
            {
                backgroundFrame.Dispose();
                backgroundFrame = null;
            }
        }

        /// <summary>
        /// 处理(当前)画面
        /// </summary>
        /// <param name="image"></param>
        public void ProcessFrame(ref Bitmap image)
        {
            if (backgroundFrame == null)
            {
                // create initial backgroung image
                backgroundFrame = grayscaleFilter.Apply(image);

                // get image dimension
                width = image.Width;
                height = image.Height;

                // just return for the first time
                return;
            }

            Bitmap tmpImage;

            // apply the the grayscale file
            tmpImage = grayscaleFilter.Apply(image);


            if (++counter == 2)
            {
                counter = 0;

                // move background towards current frame
                moveTowardsFilter.OverlayImage = tmpImage;
                moveTowardsFilter.ApplyInPlace(backgroundFrame);
            }

            // set backgroud frame as an overlay for difference filter
            differenceFilter.OverlayImage = backgroundFrame;

            // lock temporary image to apply several filters
            bitmapData = tmpImage.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            // apply difference filter
            differenceFilter.ApplyInPlace(bitmapData);
            // apply threshold filter
            thresholdFilter.ApplyInPlace(bitmapData);

            // calculate amount of changed pixels
            pixelsChanged = (calculateMotionLevel) ? CalculateWhitePixels(bitmapData) : 0;

            Bitmap tmpImage2 = openingFilter.Apply(bitmapData);

            // unlock temporary image
            tmpImage.UnlockBits(bitmapData);
            tmpImage.Dispose();

            // apply edges filter
            Bitmap tmpImage2b = edgesFilter.Apply(tmpImage2);
            tmpImage2.Dispose();

            // extract red channel from the original image
            Bitmap redChannel = extrachChannel.Apply(image);

            //  merge red channel with moving object borders
            mergeFilter.OverlayImage = tmpImage2b;
            Bitmap tmpImage3 = mergeFilter.Apply(redChannel);
            redChannel.Dispose();
            tmpImage2b.Dispose();

            // replace red channel in the original image
            replaceChannel.ChannelImage = tmpImage3;
            Bitmap tmpImage4 = replaceChannel.Apply(image);
            tmpImage3.Dispose();

            image.Dispose();
            image = tmpImage4;
        }



        private int CalculateWhitePixels(BitmapData image)
        {
            int count = 0;
            int offset = bitmapData.Stride - width;

            unsafe
            {
                byte* ptr = (byte*)bitmapData.Scan0.ToPointer();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++, ptr++)
                    {
                        count += ((*ptr) >> 7);
                    }
                    ptr += offset;
                }
            }

            return count;
        }
    }
}
