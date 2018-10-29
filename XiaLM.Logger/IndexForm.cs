using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using XiaLM.Logger.Help;
using XiaLM.Logger.Model;
using XiaLM.Logger.Realize;
using XiaLM.Logger.UserControls;

namespace XiaLM.Logger
{
    public partial class IndexForm : Form
    {
        /// <summary>
        /// 记录上一次窗体尺寸
        /// </summary>
        private Size recordFormSize;
        private bool IsStart;   //是否启动监控
        private UdpRealize server;

        public IndexForm()
        {
            InitializeComponent();
            recordFormSize = this.Size;
            server = new UdpRealize();
            server.ClientInsertEvent += UDPServer_ClientInsertEvent;
            server.MessageInsertEvent += UDPServer_MessageInsertEvent;
        }

        /// <summary>
        /// 窗体大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IndexForm_SizeChanged(object sender, EventArgs e)
        {
            FormStyleHelper.FormChangeSize(this, recordFormSize, this.Size);
            recordFormSize = this.Size;
        }

        /// <summary>
        /// 新增客户端事件
        /// </summary>
        /// <param name="obj"></param>
        private void UDPServer_ClientInsertEvent(UdpClientInfo obj)
        {
            if (obj == null) return;
            TabPage tabPage = new TabPage(obj.clientname)
            {
                Dock = DockStyle.Fill,
                ToolTipText = obj.endpoint
            };
            tabPage.Controls.Add(new MyTabPage());
            this.Invoke(new Action(() =>
            {
                this.tabControl1.TabPages.Add(tabPage); //增加TabPage
                this.comBoxClient.Items.Add($"{obj.clientname}");
                this.comBoxClient.SelectedIndex = this.tabControl1.SelectedIndex;
            }));
        }

        /// <summary>
        /// 新增消息事件
        /// </summary>
        /// <param name="ePint"></param>
        /// <param name="inMsg"></param>
        private void UDPServer_MessageInsertEvent(string ePint, InMsg inMsg)
        {
            string msgStr = $"[{inMsg.Type}][{inMsg.Time}]{inMsg.Message}|{inMsg.Exception}\r\n";
            this.Invoke(new Action(() =>
            {
                for (int i = 0; i < this.tabControl1.TabPages.Count; i++)
                {
                    if (this.tabControl1.TabPages[i].ToolTipText.Equals(ePint))
                    {
                        ((RichTextBox)this.tabControl1.TabPages[i].Controls[0].Controls[0]).Text += msgStr;
                        int txtLenth = ((RichTextBox)this.tabControl1.TabPages[i].Controls[0].Controls[0]).TextLength;  //字节长度
                        FormStyleHelper.SetCursorToTextBoxBase(((RichTextBox)this.tabControl1.TabPages[i].Controls[0].Controls[0]), txtLenth, 0);
                    }
                }
            }));
        }

        /// <summary>
        /// 启动/停止监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butStartMonitor_Click(object sender, EventArgs e)
        {
            if (IsStart)
            {
                server.StopRecive();
                this.butStartMonitor.Text = "启动监控";
                IsStart = false;
            }
            if (!IsStart)
            {
                server.StartRecive();
                this.butStartMonitor.Text = "停止监控";
                IsStart = true;
            }
        }

        /// <summary>
        /// TabControl选择发生变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comBoxClient.SelectedIndex != this.tabControl1.SelectedIndex)
            {
                this.comBoxClient.SelectedIndex = this.tabControl1.SelectedIndex;
            }
        }

        /// <summary>
        /// 选择客户端发生变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comBoxClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex != this.comBoxClient.SelectedIndex)
            {
                this.tabControl1.SelectedIndex = this.comBoxClient.SelectedIndex;
            }
        }

        /// <summary>
        /// 单个Client暂停/开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butPause_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 单个Client清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butOneClear_Click(object sender, EventArgs e)
        {
            int clientNum = this.comBoxClient.SelectedIndex;
            if (clientNum < 0) return;
            this.tabControl1.TabPages.RemoveAt(clientNum);
            this.comBoxClient.Items.RemoveAt(clientNum);
            server.RemoveClientAt(clientNum);
        }

        /// <summary>
        /// 清除全部Client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butAllClear_Click(object sender, EventArgs e)
        {
            this.tabControl1.TabPages.Clear();
            this.comBoxClient.Items.Clear();
            server.ClearClient();
        }

        /// <summary>
        /// 查看日志文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butLookLogFile_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name.Equals("LookLogForm"))
                {
                    frm.BringToFront(); //至于屏幕第一位置
                    return;
                }
            }
            string client = (string)this.comBoxClient.SelectedItem;
            string fName = $"Log/{client}_{DateTime.Now.ToString("yyyy.MM.dd.HH")}.log";
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + fName))
            {
                MessageBox.Show("未找到日志文件", "常规错误", MessageBoxButtons.OK);
                return;
            }
            new LookLogForm(AppDomain.CurrentDomain.BaseDirectory + fName).Show();
        }

        private void butSelctionLogFile_Click(object sender, EventArgs e)
        {

        }

        private void butLookAtLogFile_Click(object sender, EventArgs e)
        {

        }
    }
}
