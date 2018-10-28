using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using XiaLM.Logger.Help;

namespace XiaLM.Logger.UserControls
{
    public partial class LookLogForm : Form
    {
        /// <summary>
        /// 打开的日志文件名
        /// </summary>
        private string Fname;
        /// <summary>
        /// 记录上一次窗体尺寸
        /// </summary>
        private Size recordFormSize;
        private List<string> InfoList = new List<string>();
        private List<string> WarnList = new List<string>();
        private List<string> ErrorList = new List<string>();
        private List<string> FatalList = new List<string>();
        private List<string> DebugList = new List<string>();
        private List<string> DefaultList = new List<string>();

        public LookLogForm(string fName)
        {
            InitializeComponent();
            Fname = fName;
        }

        private void LookLogForm_Load(object sender, EventArgs e)
        {
            recordFormSize = this.Size;
            WriteLogFileToTexBox(Fname);
        }

        /// <summary>
        /// 读取日志文件写入界面文本框
        /// </summary>
        /// <param name="fName"></param>
        private void WriteLogFileToTexBox(string fName)
        {
            Task.Factory.StartNew(() =>
            {
                string[] strArray = File.ReadAllLines(fName);
                if (strArray == null || strArray.Length <= 0) return;
                DefaultList = strArray.ToList();
                this.Invoke(new Action(() =>
                {
                    this.richTextBox1.Lines = DefaultList.ToArray();
                    this.labelDefault.Text = $"{DefaultList.Count}个";
                }));

                foreach (var item in strArray)
                {
                    if (item.StartsWith("[INFO]"))
                    {
                        InfoList?.Add(item);
                    }
                    if (item.StartsWith("[WARN]"))
                    {
                        WarnList?.Add(item);
                    }
                    if (item.StartsWith("[ERROR]"))
                    {
                        ErrorList?.Add(item);
                    }
                    if (item.StartsWith("[FATAL]"))
                    {
                        FatalList?.Add(item);
                    }
                    if (item.StartsWith("[DEBUG]"))
                    {
                        DebugList?.Add(item);
                    }
                }

                this.Invoke(new Action(() =>
                {
                    this.labelInfo.Text = $"{InfoList.Count}个";
                    this.labelWarn.Text = $"{WarnList.Count}个";
                    this.labelError.Text = $"{ErrorList.Count}个";
                    this.labelFatal.Text = $"{FatalList.Count}个";
                    this.labelDubeg.Text = $"{DebugList.Count}个";
                }));
            });
        }

        /// <summary>
        /// 窗体大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookLogForm_SizeChanged(object sender, EventArgs e)
        {
            FormStyleHelper.FormChangeSize(this, recordFormSize, this.Size);
            recordFormSize = this.Size;
        }

        private void radioButtonSelectModel_CheckedChanged(object sender, EventArgs e)
        {
            switch (((RadioButton)sender).Text.Trim())
            {
                case "Default":
                    this.richTextBox1.Lines = DefaultList.ToArray();
                    break;
                case "Info":
                    this.richTextBox1.Lines = InfoList.ToArray();
                    break;
                case "Warn":
                    this.richTextBox1.Lines = WarnList.ToArray();
                    break;
                case "Error":
                    this.richTextBox1.Lines = ErrorList.ToArray();
                    break;
                case "Fatal":
                    this.richTextBox1.Lines = FatalList.ToArray();
                    break;
                case "Debug":
                    this.richTextBox1.Lines = DebugList.ToArray();
                    break;
                default:
                    break;
            }
        }


    }
}
