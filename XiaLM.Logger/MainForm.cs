using LogHelp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogHelp
{
    public partial class MainForm : Form
    {
        private bool IsStart { get; set; } = false; //是否已启动监视
        private bool IsRool { get; set; } = true; //日志是否自动滚动
        private LogPage lPage { get; set; } //当前日志页
        private static readonly object lockObj = new object();
        private static MainForm mainForm;
        public static MainForm GetInstance()
        {
            if (mainForm == null)
            {
                lock (lockObj)
                {
                    if (mainForm == null)
                    {
                        mainForm = new MainForm();
                    }
                }
            }
            return mainForm;
        }
        public MainForm()
        {
            InitializeComponent();
            UDPRealize.GetInstance().LogPageQueueEvent += MainForm_LogPageQueueEvent;
            UDPRealize.GetInstance().MesageQueueEvent += MainForm_MesageQueueEvent;
        }

        /// <summary>
        /// 日志页入队列事件
        /// </summary>
        private void MainForm_LogPageQueueEvent()
        {
            this.Invoke(new Action(() =>
            {
                this.comboBox1.Items.Clear();
                this.comboBox1.Items.AddRange(UDPRealize.GetInstance().logPages.ToArray());
                this.comboBox1.DisplayMember = "Point";
                if (this.comboBox1.Items.Count > 1)
                {
                    this.comboBox1.SelectedItem = lPage;
                }
                else
                {
                    this.comboBox1.SelectedIndex = 0;   //默认选中第一项
                    this.button2.Enabled = true;
                }
            }));
        }

        /// <summary>
        /// 日志入队列事件
        /// </summary>
        /// <param name="obj"></param>
        private void MainForm_MesageQueueEvent(LogPage obj)
        {
            if (obj.Equals(lPage))
            {
                this.Invoke(new Action(RefreshLogPage));
            }
        }

        /// <summary>
        /// 启动/关闭监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            IsStart = !IsStart;
            if (IsStart)
            {
                UDPRealize.GetInstance().StartRecive();
                this.button1.Text = "关闭监视";
            }
            else
            {
                UDPRealize.GetInstance().StopRecive();
                this.button1.Text = "启动监视";
                this.button2.Enabled = false;
            }
        }

        /// <summary>
        /// 切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lPage = UDPRealize.GetInstance().logPages.Find(p => p.Equals(this.comboBox1.SelectedItem));
            RefreshLogPage();
        }

        /// <summary>
        /// 刷新日志页
        /// </summary>
        private void RefreshLogPage()
        {
            if (IsRool)
            {
                this.richTextBox1.Lines = lPage.QMesages.Select(p => "[" + p.LogLevel + "]" + "[" + p.RTime + "]" + p.Message + DealException(p.Exception)).ToArray();
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();   //将滚动调设置到最底下
            }
        }
        /// <summary>
        /// 异常信息处理
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string DealException(Exception ex)
        {
            if (ex != null)
            {
                return "{"+ ex.Message + ":" + ex.ToString() + "}";
            }
            return null;
        }

        /// <summary>
        /// 暂停/启动日志滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            IsRool = !IsRool;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        /// <summary>
        /// 清除日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Clear();
            lPage.QMesages.Clear();
        }
    }
}
