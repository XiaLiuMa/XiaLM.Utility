using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using XiaLM.AForge;

namespace XiaLM.FormTest.Camera
{
    public partial class CameraForm : Form
    {
        private bool EnableDetectionPoker;  //是否识别扑克
        private CameraManager _CameraManager; //摄像头管理器
        public Bitmap _Bitmap = new Bitmap(1, 1);

        public CameraForm()
        {
            InitializeComponent();
            _CameraManager = new CameraManager();
            _CameraManager.RefreshBitmap += Cameraer_RefreshBitmap;
        }

        private void CameraForm_Load(object sender, EventArgs e)
        {
            //for (int i = 0; i < _CameraManager.CameraList.Count; i++)
            //{

            //}

        }

        /// <summary>
        /// 摄像头每一帧刷新
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bmp"></param>
        private void Cameraer_RefreshBitmap(int index, Bitmap bmp)
        {
            this.Invoke(new Action(() =>
            {
                switch (index)
                {
                    case 0:
                        this.pictureBox1.Image = bmp;
                        if (EnableDetectionPoker) _CameraManager.PokerDetection(bmp);
                        break;
                    case 1:
                        this.pictureBox2.Image = bmp;
                        break;
                    case 2:
                        this.pictureBox3.Image = bmp;
                        break;
                    case 3:
                        this.pictureBox4.Image = bmp;
                        break;
                }
                lock (_Bitmap)
                {
                    _Bitmap.Dispose();  //释放上一个缓存
                    _Bitmap = bmp.Clone() as Bitmap;    //保存新的缓存
                }
            }));
        }

        


        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _CameraManager.CameraList.Count; i++)
            {
                _CameraManager.Start(i);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //cameraer.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //cameraer.RecordVideo();
        }

        /// <summary>
        /// 识别扑克
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            EnableDetectionPoker = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            EnableDetectionPoker = false;
        }
    }
}
