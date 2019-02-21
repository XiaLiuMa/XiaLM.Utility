using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing;
using System.Drawing.Imaging;
using XiaLM.Log;

namespace XiaLM.MotionDetector.MotionDetector
{
    public class MotionDetectorB : IMotionDetector
    {
        private Bitmap backgroundFrame;
        private BitmapData bitmapData;
        private IFilter GrayscaleFilter = new GrayscaleBT709();
        private Difference DifferenceFilter = new Difference();
        private Threshold ThresholdFilter = new Threshold(15);
        private IFilter ErosionFilter = new Erosion();
        private Merge MergeFilter = new Merge();

        private IFilter extrachChannel = new ExtractChannel(RGB.R);
        private ReplaceChannel replaceChannel = new ReplaceChannel(RGB.R, (Bitmap)null);
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
                backgroundFrame = GrayscaleFilter.Apply(image); // create initial backgroung image

                // get image dimension
                width = image.Width;
                height = image.Height;

                // just return for the first time
                return;
            }

            Bitmap tmpImage = GrayscaleFilter.Apply(image); //灰度过滤
            // set backgroud frame as an overlay for difference filter
            DifferenceFilter.OverlayImage = backgroundFrame;
            Bitmap tmpImage2 = DifferenceFilter.Apply(tmpImage);    //差异过滤

            // lock the temporary image and apply some filters on the locked data
            bitmapData = tmpImage2.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            // threshold filter
            ThresholdFilter.ApplyInPlace(bitmapData);   //阈值过滤
            // erosion filter
            Bitmap tmpImage3 = ErosionFilter.Apply(bitmapData); //腐蚀过滤

            // unlock temporary image
            tmpImage2.UnlockBits(bitmapData);
            tmpImage2.Dispose();

            // calculate amount of changed pixels
            pixelsChanged = (calculateMotionLevel) ? CalculateWhitePixels(tmpImage3) : 0;

            // dispose old background
            backgroundFrame.Dispose();
            // set backgound to current
            backgroundFrame = tmpImage;

            // extract red channel from the original image
            Bitmap redChannel = extrachChannel.Apply(image);    //提取通道

            //  merge red channel with moving object
            MergeFilter.OverlayImage = tmpImage3;
            Bitmap tmpImage4 = MergeFilter.Apply(redChannel);   //合并过滤
            redChannel.Dispose();
            tmpImage3.Dispose();

            // replace red channel in the original image
            replaceChannel.ChannelImage = tmpImage4;
            Bitmap tmpImage5 = replaceChannel.Apply(image); //替换通道
            tmpImage4.Dispose();

            image.Dispose();
            image = tmpImage5;
        }



        private int CalculateWhitePixels(Bitmap image)
        {
            int count = 0;
            BitmapData data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed); // lock difference image
            int offset = data.Stride - width;
            unsafe
            {
                try
                {
                    byte* ptr = (byte*)data.Scan0.ToPointer();
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++, ptr++)
                        {
                            count += ((*ptr) >> 7);
                        }
                        ptr += offset;
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.Error(ex);
                }
            }

            image.UnlockBits(data); // unlock image
            return count;
        }
    }
}
