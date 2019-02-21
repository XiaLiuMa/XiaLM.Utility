using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using System;
using System.Drawing;

namespace XiaLM.AForge
{
    public class Camera11111
    {
        /// <summary>
        /// 用来操作摄像头
        /// </summary>
        private VideoCaptureDevice Camera = null;
        /// <summary>
        /// 用来把每一帧图像编码到视频文件
        /// </summary>
        private VideoFileWriter VideoOutPut = new VideoFileWriter();
        /// <summary>
        /// 图像缓存
        /// </summary>
        private Bitmap bmp = new Bitmap(1, 1);
        public event Action<Bitmap> RefreshBitmap = (p) => { };
        /// <summary>
        /// 摄像头驱动列表
        /// </summary>
        public FilterInfoCollection Devices
        {
            get { return new FilterInfoCollection(FilterCategory.VideoInputDevice); }
        }

        /// <summary>
        /// 根据索引打开相机
        /// </summary>
        /// <param name="index"></param>
        public void Start(int index = 0)
        {
            Camera = new VideoCaptureDevice(Devices[index].MonikerString);  //通过索引找到指定摄像头
            Camera.VideoResolution = Camera.VideoCapabilities[0];   ////配置录像参数(宽,高,帧率,比特率等参数)
            Camera.NewFrame += Camera_NewFrame; //设置回调,aforge会不断从这个回调推出图像数据
            Camera.Start(); //打开摄像头
            VideoOutPut.Open("E:/VIDEO.MP4",
                Camera.VideoResolution.FrameSize.Width,
                Camera.VideoResolution.FrameSize.Height,
                Camera.VideoResolution.AverageFrameRate,
                VideoCodec.MPEG4,
                Camera.VideoResolution.BitCount);    //打开录像文件(如果没有则创建,如果有也会清空)
        }

        /// <summary>
        /// 录制视频到文件
        /// </summary>
        /// <param name="filePath"></param>
        public void RecordVideo(string filePath = "E:/VIDEO.MP4")
        {
            VideoOutPut.Open(filePath,
                Camera.VideoResolution.FrameSize.Width,
                Camera.VideoResolution.FrameSize.Height,
                Camera.VideoResolution.AverageFrameRate,
                VideoCodec.MPEG4,
                Camera.VideoResolution.BitCount);    //打开录像文件(如果没有则创建,如果有也会清空)
        }

        /// <summary>
        /// 摄像头输出回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void Camera_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            VideoOutPut.WriteVideoFrame(eventArgs.Frame);   //写到文件
            lock (bmp)
            {
                bmp.Dispose();  //释放上一个缓存
                bmp = eventArgs.Frame.Clone() as Bitmap;    //保存新的缓存
                RefreshBitmap(bmp);
            }
        }

        public void Close()
        {
            Camera.Stop();  //停止摄像头
            VideoOutPut.Close();    //关闭录像文件,如果忘了不关闭,将会得到一个损坏的文件,无法播放
        }
    }
}
