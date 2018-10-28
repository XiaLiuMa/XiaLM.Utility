using System;
using System.Windows.Forms;
using XiaLM.Can;

namespace XiaLM.FormTest.CAN
{
    public partial class CanForm : Form
    {
        private CanHelper canHelper;
        public CanForm()
        {
            InitializeComponent();
        }

        private void CanForm_Load(object sender, EventArgs e)
        {
            canHelper = new CanHelper();
            canHelper.ReceviedData += CanHelper_ReceviedData;
        }

        private void CanHelper_ReceviedData(object sender, CanFrameInfoArgs e)
        {
            
        }

        private void butSetCAN_Click(object sender, EventArgs e)
        {
            canHelper?.CanSetting.SetCAN(
                (uint)0,    //deviceIndex 设备索引号
                (byte)0,	//canIndex CAN的路索引号
                (byte)CanFilterType.DualFilter,		//filterType 过滤类型
                (byte)CanMode.NormalMode,   //canMode CAN模式
                "00000000",     //canCode 过滤码
                "FFFFFFFF",     //canMask 掩码
                "4F",   //时间高位
                "2F");	//时间低位,默认 4F 2F
        }

        private void butConnectCAN_Click(object sender, EventArgs e)
        {
            canHelper?.ConnectCANDevice(); //连接CAN设备
        }

        private void butStartCAN_Click(object sender, EventArgs e)
        {
            canHelper?.StartCAN();		//启动CAN设备
        }

        private void butCloseCAN_Click(object sender, EventArgs e)
        {
            canHelper?.CloseCANDevice();	//关闭CAN设备
        }

        private void butResetCAN_Click(object sender, EventArgs e)
        {
            canHelper?.ResetCANDevice();	//复位CAN设备
        }

        private void butCANSend_Click(object sender, EventArgs e)
        {
            canHelper?.SendData(
                this.tstFrameID.Text.Trim(),
                this.txtTimeStamp.Text.Trim(),
                int.Parse(this.txtFrameFormat.Text.Trim()),
                int.Parse(this.txtFrameType.Text.Trim()),
                this.txtFrameData.Text.Trim(),
                int.Parse(this.txtFrameSendType.Text.Trim())
                );
        }
    }
}
