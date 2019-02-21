using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using XiaLM.Tool450.source.common;

namespace XiaLM.FormTest.Serial
{
    public partial class SerialForm : Form
    {
        private SerialHelper serial;
        public SerialForm()
        {
            InitializeComponent();
            serial = new SerialHelper(new SerialHelper.ComParms()
            {
                PortName = "com5",
                BaudRate = 115200,
                Parity = SerialHelper.Parity.None,
                DataBits = 8,
                StopBits = SerialHelper.StopBits.One
            });
            serial.DataReceived += Serial_DataReceived;

        }

        private void Serial_DataReceived(byte[] bytes)
        {
            string str = ConvertHelper.BytesToX2Str(bytes);
            this.Invoke(new Action(() =>
            {
                this.richTextBox1.Text += str + "\r\n";
            }));
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            serial.Open();
            Thread.Sleep(1000);
            if(serial.IsOpen) this.richTextBox1.Text += "已启动.\r\n";
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            serial.Close();
            Thread.Sleep(1000);
            if (!serial.IsOpen) this.richTextBox1.Text += "已关闭.\r\n";
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string str = this.textBox1.Text.Trim();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            serial.Send(bytes);
        }

    }
}
