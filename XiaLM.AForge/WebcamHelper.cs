using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.AForge
{
    public class WebcamHelper
    {
        public string[] videoDeviceNames
        {
            get
            {
                List<string> devices = new List<string>();
                var cameraDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach (FilterInfo cameraDevice in cameraDevices)
                {
                    devices.Add(cameraDevice.Name);
                }
                return devices.ToArray();
            }
        }
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice WebCam;
        private int pic_NewWidth;       ///--指定图片的宽度，
        private int pic_NewHeight;      ///---指定图片的高度度
        private PictureBox picturebox;  ///--承载图像
        private string filepath;            ///---保存图片的路径,  路径中不需要指明 图片的格式。
        private Bitmap bitmap;          ///--保存抓拍截图


        public WebcamHelper()
        {

        }
        ///--构造函数
        ///--picturebox是承载图像的，
        ///--picturepath 是抓图的存储路径，
        ///---newWidth 是指定存图的宽度
        ///---newHeight 是指定存图的高度
        public WebcamHelper(ref PictureBox pictureBox, string picturePath, int newWidth, int newHeight)
        {
            ///--指定图片缩放的宽度和高度
            this.pic_NewHeight = newHeight;
            this.pic_NewWidth = newWidth;
            ///--图片路径
            this.filepath = picturePath;
            ///---picturebox,
            this.picturebox = new PictureBox();
            this.picturebox = pictureBox;
        }

        /// <summary>
        /// 选择摄像头输入设备；保存图片的路径
        /// </summary>
        /// <param name="videoDeviceName"></param>
        public void Open(string videoDeviceName = null)
        {
            if (videoDeviceNames.Length <= 0) throw new Exception("当前没有可用的摄像头输入设备");
            if (string.IsNullOrEmpty(videoDeviceName))
            {
                this.WebCam = new VideoCaptureDevice(videoDevices[0].MonikerString);
            }
            else
            {
                if (!videoDeviceNames.Contains(videoDeviceName)) throw new Exception("列表中无指定的摄像头输入设备");
                this.WebCam = new VideoCaptureDevice(videoDeviceName);
            }

            /////--枚举可用的摄像头输入设备
            //this.videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //if (0 == videoDevices.Count)
            //{
            //    ///---没有可用的输入设备
            //    throw new Exception("当前没有可用的摄像头输入设备");
            //}
            /////--保存当前输入设备的名称
            //string[] videoDevicesName = new string[videoDevices.Count];
            /////---计数器
            //int count = 0;
            /////---获取设备名称
            //foreach (FilterInfo item in videoDevices)
            //{
            //    ///--将设备名称返回到数组中
            //    videoDevicesName[count++] = item.Name;
            //}
            /////--计数器清零
            //count = 0;
            /////---判断传入的指定的设备名是否为空
            /////---为空，默认使用第一个设备
            //if (string.Empty == videoDeviceName)
            //{
            //    this.WebCam = new VideoCaptureDevice(videoDevices[0].MonikerString);
            //}
            //else
            //{
            //    ///-设备索引
            //    int index = 0;
            //    ///--使用指定的输入设备名称
            //    ///---查找当前指定的输入设备在输入设备中的索引
            //    for (int i = 0; i < videoDevices.Count; i++)
            //    {
            //        if (videoDeviceName == videoDevicesName[i])
            //        {
            //            index = i;
            //            break;
            //        }
            //    }
            //    ///---连接指定的设备
            //    this.WebCam = new VideoCaptureDevice(videoDevices[index].MonikerString);
            //}
            ///--设定摄像头的分辨率为默认使用的分辨率
            this.WebCam.VideoResolution = this.WebCam.VideoCapabilities[0];
            ///--打开摄像头
            this.WebCam.Start();
            this.WebCam.NewFrame += new NewFrameEventHandler(WebcamNewFrameCallBack);
        }
        ///---回调函数
        private void WebcamNewFrameCallBack(object obj, NewFrameEventArgs eventArgs)
        {
            bitmap = (Bitmap)eventArgs.Frame.Clone();
            this.picturebox.Image = bitmap;
            GC.Collect();
        }

        ///--函数功能:抓拍图片，并保存
        ///---成功返回：true. 失败返回：false、
        public bool Capture()
        {
            if (bitmap != null)
            {
                ///--缩放图片
                Zoom(ref this.bitmap, this.pic_NewWidth, this.pic_NewHeight).Save(this.filepath + ".jpg");
                return true;
            }
            else
            {
                ///--存储失败
                return false;
            }
        }
        ///--函数功能：实现抓拍的图像缩放
        ///---返回缩放的图片。
        private Bitmap Zoom(ref Bitmap bitmap, int new_Width, int new_Height)
        {
            ///--新建一个bitmap对象
            Bitmap newBitmap = new Bitmap(new_Width, new_Width);
            Graphics newG = Graphics.FromImage(newBitmap);
            ///--插入算法的质量
            newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            ///--重绘
            newG.DrawImage(bitmap, new Rectangle(0, 0, new_Width, new_Width), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
            ///--释放绘图工具
            newG.Dispose();
            ///---返回新结果
            return newBitmap;
        }


        ///---函数功能：断开摄像头连接
        ///---参数，无
        public void Close()
        {
            if (this.WebCam != null)
            {
                if (this.WebCam.IsRunning)
                {
                    this.WebCam.Stop();
                }
            }
        }

    }
}
