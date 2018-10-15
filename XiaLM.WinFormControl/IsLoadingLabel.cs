using System;
using System.Windows.Forms;

namespace XiaLM.WinFormControl
{
    /// <summary>
    /// 正在运行
    /// </summary>
    public partial class IsLoadingLabel : Label
    {
        Timer labelTimer;   //定时器
        int counter = 0;    //计数器
        string tempStr; //label文字
        Char[] charArray;   //把Label文字转化成字符数组

        public IsLoadingLabel()
        {
            InitializeComponent();
            tempStr = this.Text.Trim();
            charArray = tempStr.ToCharArray();

            InitTimer();
            labelTimer.Tick += new EventHandler(LabelTimerEvent);  
        }

        /// <summary>
        /// 初始化定时器
        /// </summary>
        public void InitTimer()
        {
            labelTimer = new Timer();   //定时器
            labelTimer.Interval = this.Interval;
            labelTimer.Enabled = true;
        }

        /// <summary>
        /// 定时器触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelTimerEvent(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Text))
            {
                counter += 1;
                if (counter > charArray.Length)
                {
                    counter = 0;
                }
                this.Text = charArray.GetValue(0, counter).ToString();
            }
        }

        /// <summary>
        /// 频率(单位是毫秒)【外增属性】
        /// </summary>
        private int interval = 5000;
        public int Interval
        {
            get { return interval; }
            set { interval = value;}
        }
    }
}
