using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tool450.source.xml_sample
{
    /// <summary>
    /// 配置文件模型
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 机器人名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 唤醒词
        /// </summary>
        public string WakeupWords { get; set; }
        /// <summary>
        /// 机器人中文自我介绍信息
        /// </summary>
        public List<string> SelfInfosCN { get; set; }
        /// <summary>
        /// 机器人英文自我介绍信息
        /// </summary>
        public List<string> SelfInfosEN { get; set; }
        /// <summary>
        /// 声卡名称
        /// </summary>
        public string AudioCardName { get; set; }
        /// <summary>
        /// 地图设置
        /// </summary>
        public MapSetting MapSetting { get; set; }
        /// <summary>
        /// 打印机配置文件【暂时没用到】
        /// </summary>
        public PrinterConfig printerConfig { get; set; }
        /// <summary>
        /// 可变配置
        /// </summary>
        public VariableLayout VariableLayout { get; set; }
        /// <summary>
        /// 设备
        /// </summary>
        public Device Device { get; set; }
    }
    /// <summary>
    /// 地图设置
    /// </summary>
    public class MapSetting
    {
        /// <summary>
        /// 是否开启地图充电
        /// </summary>
        public bool IsOpenMapCharge { get; set; }
        /// <summary>
        /// 低电量警戒值
        /// </summary>
        public int LowPower { get; set; }
    }
    public class PrinterConfig
    {
        //页边距(上、下、左、右)
        public int topMargin { get; set; }
        public int bottomMargin { get; set; }
        public int leftMargin { get; set; }
        public int rightMargin { get; set; }
    }
    /// <summary>
    /// 可变配置，用于后期修改系统音量，发音人，语速，服务器地址，软件说明
    /// </summary>
    public class VariableLayout
    {
        /// <summary>
        /// 语种
        /// </summary>
        public string Informant { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 音量
        /// </summary>
        public int Volume { get; set; }
        /// <summary>
        /// 语速
        /// </summary>
        public int Speed { get; set; }
        /// <summary>
        /// 服务器ip
        /// </summary>
        public string ServerIP { get; set; }
        /// <summary>
        /// 服务器端口号
        /// </summary>
        public string ServerPort { get; set; }
        /// <summary>
        /// 地图服务-机器人IP
        /// </summary>
        public string MapLocalIp { get; set; }
        /// <summary>
        /// 地图服务-机器人端口号
        /// </summary>
        public string MapLocalPort { get; set; }
        /// <summary>
        /// 扫描仪-串口号
        /// </summary>
        public ushort ScannerSeralPort { get; set; }
        /// <summary>
        /// 扫描仪-波特率
        /// </summary>
        public uint ScannerBaudRate { get; set; }
        /// <summary>
        /// 软件说明
        /// </summary>
        public string Explain { get; set; }
        /// <summary>
        /// 下载和导入的资源保存路径
        /// </summary>
        public string ResourcePath { get; set; }
        /// <summary>
        /// 对其他设备显示的名称
        /// </summary>
        public string ExternalName { get; set; }
        /// <summary>
        /// 更新程序启动路径
        /// </summary>
        public string UpdateExePath { get; set; }
    }
    public class Device
    {
        /// <summary>
        /// 主控板串口号
        /// </summary>
        public string MianPanelProt { get; set; }
        /// <summary>
        /// 主控板波特率
        /// </summary>
        public int MianPanelBaudRate { get; set; }

        /// <summary>
        /// 胸控板串口号
        /// </summary>
        public string ChestPanelProt { get; set; }
        /// <summary>
        /// 胸控板波特率
        /// </summary>
        public int ChestPanelBaudRate { get; set; }

        /// <summary>
        /// GSM模块串口号
        /// </summary>
        public string GsmPanelPort { get; set; }
        /// <summary>
        /// GSM模块波特率
        /// </summary>
        public int GsmPanelBaudRate { get; set; }
        /// <summary>
        /// 默认电话号码
        /// </summary>
        public string GsmDefaultPhoneNum { get; set; }
    }
    
}
