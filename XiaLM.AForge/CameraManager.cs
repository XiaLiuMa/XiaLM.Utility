using AForge.Video;
using AForge.Video.DirectShow;
using System.Linq;
using System;
using System.Drawing;
using System.Collections.Generic;
using XiaLM.Log;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge;
using AForge.Math.Geometry;

namespace XiaLM.AForge
{
    /// <summary>
    /// 相机管理器
    /// </summary>
    public class CameraManager
    {
        /// <summary>
        /// 摄像头列表
        /// </summary>
        public List<VideoCaptureDevice> CameraList;
        /// <summary>
        /// 图像缓存列表
        /// </summary>
        public List<Bitmap> BitmapList;
        /// <summary>
        /// 摄像头(指定索引摄像)每一帧刷新事件
        /// </summary>
        public event Action<int, Bitmap> RefreshBitmap = (i, b) => { };

        public CameraManager()
        {
            try
            {
                var drices = new FilterInfoCollection(FilterCategory.VideoInputDevice); //驱动列表
                CameraList = new List<VideoCaptureDevice>();
                BitmapList = new List<Bitmap>();
                for (int i = 0; i < drices.Count; i++)
                {
                    var obj = new VideoCaptureDevice(drices[i].MonikerString);
                    //obj.VideoResolution = obj.VideoCapabilities[0];   //配置录像参数(宽,高,帧率,比特率等参数)
                    obj.NewFrame += Camera_NewFrame; //设置回调,aforge会不断从这个回调推出图像数据
                    CameraList.Add(obj);
                    BitmapList.Add(new Bitmap(1, 1));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "加载相机管理器异常");
            }
        }

        /// <summary>
        /// 摄像头输出回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void Camera_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var videoCapture = sender as VideoCaptureDevice;
            int index = CameraList.IndexOf(videoCapture);
            lock (BitmapList[index])
            {
                BitmapList[index].Dispose();  //释放上一个缓存
                BitmapList[index] = eventArgs.Frame.Clone() as Bitmap;    //保存新的缓存
                RefreshBitmap(index, BitmapList[index]);
            }
        }

        /// <summary>
        /// 根据索引打开相机
        /// </summary>
        /// <param name="index"></param>
        public void Start(int index = 0)
        {
            CameraList[index].Start(); //打开摄像头
        }

        public void Close(int index = 0)
        {
            CameraList[index].Stop();  //停止摄像头
        }

        /// <summary>
        /// 扑克识别
        /// </summary>
        /// <param name="bmp"></param>
        public void PokerDetection(Bitmap bmp)
        {
            FiltersSequence seq = new FiltersSequence();
            seq.Add(Grayscale.CommonAlgorithms.BT709);  // 添加灰度滤镜
            seq.Add(new OtsuThreshold()); // 添加二值化滤镜
            bmp = seq.Apply(bmp); // 应用滤镜

            // 从图像中提取宽度和高度大于150的blob
            BlobCounter extractor = new BlobCounter();
            extractor.FilterBlobs = true;
            extractor.MinWidth = extractor.MinHeight = 150;
            extractor.MaxWidth = extractor.MaxHeight = 350;
            extractor.ProcessImage(bmp);

            // 用于从原始图像提取扑克牌
            QuadrilateralTransformation quadTransformer = new QuadrilateralTransformation();
            int CardWidth = 200;
            int CardHeight = 300;
            // 用于调整扑克牌大小
            ResizeBilinear resizer = new ResizeBilinear(CardWidth, CardHeight);

            foreach (Blob blob in extractor.GetObjectsInformation())
            {
                // 获取扑克牌的边缘点
                List<IntPoint> edgePoints = extractor.GetBlobsEdgePoints(blob);
                // 利用边缘点，在原始图像上找到四角
                List<IntPoint> corners = PointsCloud.FindQuadrilateralCorners(edgePoints);
                Bitmap cardImg = quadTransformer.Apply(bmp); // 提取扑克牌图像

                if (cardImg.Width > cardImg.Height) // 如果扑克牌横放
                {
                    cardImg.RotateFlip(RotateFlipType.Rotate90FlipNone); // 旋转之
                }
                cardImg = resizer.Apply(cardImg); // 归一化（重设大小）扑克牌
                RefreshBitmap(3, cardImg);
            }

        }



        //private void InitVideoParam(VideoProcAmpProperty cp, string vslidername, bool Check)
        //{
        //    VideoAMPProperty vpp = (VideoAMPProperty)camera_Chemistry.videoAMPProperty[cp];
        //    VideoSlider vs = new VideoSlider(camera_Chemistry);
        //    vs.Name = vslidername;
        //    vs.VideoProcAmpPropertyItemName = vslidername;
        //    if (vpp != null)
        //    {
        //        vs.AmpProperty = vpp;
        //    }
        //    stcvediosliders.Children.Add(vs);
        //}

        //private void GanHuaxueCamera(VideoCaptureDevice vcd)
        //{
        //    camera_Chemistry.Cameratype = CameraType.USB;
        //    camera_Chemistry.Usage = CameraUsage.干化学;
        //    camera_Chemistry.videoDevice = UsbVideo;
        //    camera_Chemistry.ReadProperty(UsbVideo);
        //    stcvediosliders.Children.Clear();
        //    InitVideoParam(VideoProcAmpProperty.VideoProcAmp_Brightness, "亮度", true);
        //    InitVideoParam(VideoProcAmpProperty.VideoProcAmp_Gain, "增益", true);
        //    InitVideoParam(VideoProcAmpProperty.VideoProcAmp_Contrast, "对比度", true);
        //    InitVideoParam(VideoProcAmpProperty.VideoProcAmp_Hue, "色调", true);
        //    InitVideoParam(VideoProcAmpProperty.VideoProcAmp_Saturation, "饱和度", true);
        //    InitVideoParam(VideoProcAmpProperty.VideoProcAmp_Sharpness, "清晰度", true);
        //    InitVideoParam(VideoProcAmpProperty.VideoProcAmp_Gamma, "伽马值", true);
        //    InitVideoParam(VideoProcAmpProperty.VideoProcAmp_WhiteBalance, "白平衡", true);
        //    InitVideoParam(VideoProcAmpProperty.VideoProcAmp_BacklightCompensation, "逆光对比", true);
        //    InitVideoParam(VideoProcAmpProperty.VideoProAmp_Red, "红色分量", true);
        //    InitVideoParam(VideoProcAmpProperty.VideoProAmp_Green, "绿色分量", true);
        //    InitVideoParam(VideoProcAmpProperty.VideoProAmp_Blue, "蓝色分量", true);
        //    InitCameraParam(CameraControlProperty.Pan, "全景", true);
        //    InitCameraParam(CameraControlProperty.Iris, "光圈", true);
        //    InitCameraParam(CameraControlProperty.Roll, "Roll_", true);
        //    InitCameraParam(CameraControlProperty.Tilt, "倾斜", true);
        //    InitCameraParam(CameraControlProperty.Exposure, "曝光", true);
        //    InitCameraParam(CameraControlProperty.Focus, "焦点", true);
        //    InitCameraParam(CameraControlProperty.Zoom, "缩放", true);
        //    InitCameraParam(CameraControlProperty.Roll, "滚动", true);
        //    InitCameraParam(CameraControlProperty.LowCompensation, "低亮度补偿", true);
        //    InitCameraParam(CameraControlProperty.DeNoise, "降噪", true);
        //}
        //private void InitCameraParam(CameraControlProperty cp, string vslidername, bool Check)
        //{
        //    CameraProperty vpp = (CameraProperty)camera_Chemistry.CameraProperty[cp];
        //    if (vpp == null) return;
        //    VideoSlider vs = new VideoSlider(camera_Chemistry);
        //    vs.Name = vslidername;
        //    vs.VideoProcAmpPropertyItemName = vslidername;
        //    if (vpp != null)
        //    {
        //        vs.CameraPropertyCustom = vpp;
        //    }
        //    stccanmreraliders.Children.Add(vs);
        //}
    }
}
