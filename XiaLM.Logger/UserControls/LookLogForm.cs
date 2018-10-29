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
            WriteLogFileToTexBox(Fname).Employ();
        }

        /// <summary>
        /// 读取日志文件写入界面文本框
        /// </summary>
        /// <param name="fName"></param>
        private async Task WriteLogFileToTexBox(string fName)
        {
            await Task.Factory.StartNew(() =>
            {
                string[] strArray = File.ReadAllLines(fName);
                if (strArray == null || strArray.Length <= 0) return;
                DefaultList = strArray.ToList();
                this.Invoke(new Action(() =>
                {
                    this.richTextBox1.Lines = DefaultList.ToArray();
                }));

                string temStr = string.Empty;   //临时字符串
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].StartsWith("[INFO]")
                        || strArray[i].StartsWith("[WARN]")
                        || strArray[i].StartsWith("[ERROR]")
                        || strArray[i].StartsWith("[FATAL]")
                        || strArray[i].StartsWith("[DEBUG]"))
                    {
                        StrClassify(temStr);
                        temStr = strArray[i];
                    }
                    else
                    {
                        temStr += "\r\n" + strArray[i];
                    }
                }
                StrClassify(temStr);    //处理最后一行
                this.Invoke(new Action(() =>
                {
                    this.labelInfo.Text = $"{InfoList.Count}个";
                    this.labelWarn.Text = $"{WarnList.Count}个";
                    this.labelError.Text = $"{ErrorList.Count}个";
                    this.labelFatal.Text = $"{FatalList.Count}个";
                    this.labelDubeg.Text = $"{DebugList.Count}个";
                    int defaultCount = InfoList.Count + WarnList.Count + ErrorList.Count + FatalList.Count + DebugList.Count;
                    this.labelDefault.Text = $"{defaultCount}个";
                }));
            });
        }

        /// <summary>
        /// 字符串分类
        /// </summary>
        /// <param name="txt"></param>
        private void StrClassify(string txt)
        {
            if (string.IsNullOrEmpty(txt)) return;
            if (txt.StartsWith("[INFO]"))
            {
                InfoList.Add(txt);
            }
            if (txt.StartsWith("[WARN]"))
            {
                WarnList.Add(txt);
            }
            if (txt.StartsWith("[ERROR]"))
            {
                ErrorList.Add(txt);
            }
            if (txt.StartsWith("[FATAL]"))
            {
                FatalList.Add(txt);
            }
            if (txt.StartsWith("[DEBUG]"))
            {
                DebugList.Add(txt);
            }
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
