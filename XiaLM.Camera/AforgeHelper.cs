using AForge.Video.DirectShow;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace XiaLM.Camera
{
    public class AforgeHelper
    {
        ///查找所有摄像头设备
        private void loadCameraList()
        {
            //VideoCaptureDevice cameraDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //if (cameraDevices.Count == 0)
            //{
            //    capture_btn.Enabled = false;
            //    cameraId_cob.Enabled = false;
            //    preview_btn.Enabled = false;
            //    guide_lab.Text = noCameraDevice;
            //    cameraDevices = null;

            //}
            //else if (cameraDevices.Count == 1)
            //{
            //    isSingleCamera = true;
            //    preview_btn.Enabled = false;
            //    guide_lab.Visible = false;
            //}
            //foreach (FilterInfo cameraDevice in cameraDevices)
            //{
            //    cameraId_cob.Items.Add(cameraDevice.Name);
            //    cameraId_cob.SelectedIndex = 0;
            //}
        }

        ///根据选中摄像头打开该摄像头并预览
        private VideoCaptureDevice cameraDevice;
        private void preview()
        {
            //if (null != cameraDevice)
            //{//在2个或以上摄像头进行切换时执行
            //    preview_player.SignalToStop();
            //    preview_player.WaitForStop();
            //}
            //cameraDevice =
            //new VideoCaptureDevice(cameraDevices[cameraId_cob.SelectedIndex].MonikerString);
            //cameraDevice.DesiredFrameSize = new Size(320, 240);
            //cameraDevice.DesiredFrameRate = 1;
            //preview_player.VideoSource = cameraDevice;
            //preview_player.Start();
        }

        ///记录当前帧保存为图像
        private void takePhoto()
        {
            //if (cameraDevice == null)
            //    return;
            //Bitmap bitmap = preview_player.GetCurrentVideoFrame();
            //string fullPath = Application.StartupPath + "\\";
            //if (!Directory.Exists(fullPath))
            //    Directory.CreateDirectory(fullPath);
            //string img = fullPath + "Aforge.jpg";
            //bitmap.Save(img);
            //guide_lab.Text = img;
            //guide_lab.Visible = true;
        }
    }
}
