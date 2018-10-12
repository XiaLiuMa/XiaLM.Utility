using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XiaLM.Tool450.source
{
    /// <summary>
    /// 串口通信实用类
    /// </summary>
    public class UtilitySerial
    {
        private SerialPort serialPort;
        private ComParms _comParms;
        /// <summary>
        /// 接收
        /// </summary>
        public event Action<byte[]> DataReceived = (p) => { };
        /// <summary>
        /// 发送事件
        /// </summary>
        public event Action<byte[]> SendEvent = (p) => { };
        /// <summary>
        /// 串口是否打开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return serialPort.IsOpen;
            }
        }
        public UtilitySerial(ComParms comParms)
        {
            _comParms = comParms;
            serialPort = new SerialPort();
            serialPort.PortName = _comParms.PortName;
            serialPort.BaudRate = _comParms.BaudRate;
            serialPort.Parity = (System.IO.Ports.Parity)_comParms.Parity;
            serialPort.DataBits = _comParms.DataBits;
            serialPort.StopBits = (System.IO.Ports.StopBits)_comParms.StopBits;
            serialPort.DataReceived += (s, e) =>
            {
                byte[] bs = new byte[serialPort.BytesToRead];
                serialPort.Read(bs, 0, bs.Length);
                if (DataReceived != null)
                {
                    DataReceived(bs);
                }
            };
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public bool Open(Action<string> error = null)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (error != null)
                {
                    error(ex.ToString());
                }
            }
            return false;
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            try
            {
                serialPort.Close();
                serialPort.Dispose();
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="error"></param>
        public void Send(byte[] bs, Action<string> error = null)
        {
            SendEvent(bs);
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Write(bs, 0, bs.Length);
                }
                catch (Exception ex)
                {
                    if (error != null)
                    {
                        error(ex.ToString());
                    }
                }
            }
        }

        #region 类
        /// <summary>
        /// 串口参数
        /// </summary>
        public class ComParms
        {
            /// <summary>
            /// 串口名
            /// </summary>
            public string PortName { get; set; }
            /// <summary>
            /// 比特率
            /// </summary>
            public int BaudRate { get; set; }
            /// <summary>
            /// 摘要:
            /// 获取或设置奇偶校验检查协议。
            /// 返回结果:
            /// 枚举值之一，表示奇偶校验检查协议。 默认值为 System.IO.Ports.Parity.None。
            /// </summary>
            [DefaultValue(Parity.None)]
            public Parity Parity { get; set; }
            /// <summary>
            /// 获取或设置每个字节的标准数据位长度。
            /// </summary>
            [DefaultValue(8)]
            public int DataBits { get; set; }
            /// <summary>
            /// 获取或设置每个字节的标准停止位数。
            /// </summary>
            [DefaultValue(StopBits.One)]
            public StopBits StopBits { get; set; }

        }
        /// <summary>
        ///  指定 System.IO.Ports.SerialPort 对象的奇偶校验位。
        /// </summary>
        public enum Parity
        {
            //
            // 摘要:
            //     不发生奇偶校验检查。
            None = 0,
            //
            // 摘要:
            //     设置奇偶校验位，使位数等于奇数。
            Odd = 1,
            //
            // 摘要:
            //     设置奇偶校验位，使位数等于偶数。
            Even = 2,
            //
            // 摘要:
            //     将奇偶校验位保留为 1。
            Mark = 3,
            //
            // 摘要:
            //     将奇偶校验位保留为 0。
            Space = 4
        }
        /// <summary>
        /// 摘要: 指定在 System.IO.Ports.SerialPort 对象上使用的停止位的数目。
        /// </summary>  
        public enum StopBits
        {
            /// <summary>
            /// 不使用停止位。System.IO.Ports.SerialPort.StopBits 属性不支持此值。
            /// </summary>
            None = 0,
            /// <summary>
            ///  使用一个停止位。
            /// </summary>  
            One = 1,
            /// <summary>
            /// 使用两个停止位。
            /// </summary>
            Two = 2,
            /// <summary>
            ///   使用 1.5 个停止位。
            /// </summary>
            OnePointFive = 3,
        }
        #endregion
    }
}
