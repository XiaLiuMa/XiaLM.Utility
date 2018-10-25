using System;
using System.Text;
using System.Threading;
using XiaLM.Can.Api;
using XiaLM.Can.Help;

namespace XiaLM.Can
{
    public class CanHelper
    {
        #region CAN
        //CAN connection flag
        static bool _mConnected = false;
        //CAN 设备类型
        static private uint _mDeviceType = (uint)CanApi.PCIDeviceType.VCI_USBCAN2;
        //打开CAN返回码
        static uint _openCode = 0;
        //init can 返回码
        static uint _initCode = 0;
        //CAN settings
        CanSetting _canSetting = new CanSetting();
        //DeviceIndex
        uint _DeviceIndex = 0;
        //CanIndex
        uint _CanIndex = 0;
        public CanSetting CanSetting
        {
            get { return _canSetting; }
            set
            {
                _canSetting = value;
                _DeviceIndex = _canSetting.DeviceIndex;
                _CanIndex = _canSetting.CanIndex;
            }
        }

        #endregion

        #region CAN Receive Frames

        //CAN A Frame
        static CanApi.VCI_CAN_OBJ _CanRawFrame = new CanApi.VCI_CAN_OBJ();
        //string builder
        //timestamp
        static string _timestamp = string.Empty;
        //framID
        static string _frameID = string.Empty;
        //frameFormat 
        static string _frameFormat = string.Empty;
        //FrameType
        static string _frameType = string.Empty;
        //data
        static string _frameData = string.Empty;
        //config
        static CanApi.VCI_INIT_CONFIG _CanConfig = new CanApi.VCI_INIT_CONFIG();
        //接收数据帧拼接使用
        static StringBuilder _stringBuilder = new StringBuilder("");
        #endregion

        #region  CAN Send Frames
        //CAN数据帧包装器
        CanFrameWrapper _frameWrapper = new CanFrameWrapper();

        #endregion

        #region CAN Filter
        //CAN Filter
        static CanFilter _CanFilter = new CanFilter("**");
        #endregion
        //buffer
        CanListManager _ReceviedList = new CanListManager();
        #region 接收事件
        /// <summary>
        /// 接收到有效CAN数据
        /// </summary>
        public event EventHandler<CanFrameInfoArgs> ReceviedData;
        #endregion

        #region 接收线程函数
        //接收线程
        Thread _CanReceviedThread;
        //通知委托
        delegate void OnReceviedCanInfo(CanFrameInfoArgs CanInfo);
        OnReceviedCanInfo OnReceviedCanInfoDelegate;
        delegate void OnReceviedCanMsg(string _frameID, string _timestamp, string _frameFormat, string _frameType, string _frameData);
        OnReceviedCanMsg OnReceviedCanMsgDelegate;
        public Thread CanReceviedThread
        {
            get { return _CanReceviedThread; }
        }

        /// <summary>
        ///  接收数据处理函数
        /// </summary>
        void ReceiveDataProc()
        {
            try
            {
                //接收到数据长度
                int _receivedLen = 0;
                //CAN ERR INFO
                CanApi.VCI_ERR_INFO _CanErrInfo = new CanApi.VCI_ERR_INFO();
                _CanErrInfo.Passive_ErrData = new byte[3];
                //CAN Frames
                CanApi.VCI_CAN_OBJ[] _CanRawFrames = new CanApi.VCI_CAN_OBJ[50];
                while (true)
                {
                    Thread.Sleep(10);
                    if (!_mConnected)
                        break;
                    _receivedLen = 0;
                    _receivedLen = (int)CanApi.VCI_Receive(
                        _mDeviceType, _DeviceIndex,
                        _CanIndex,
                        ref _CanRawFrames[0], 50, 200);
                    if (_receivedLen <= 0)
                    {
                        //注意：如果没有读到数据则必须调用此函数来读取出当前的错误码，
                        //千万不能省略这一步（即使你可能不想知道错误码是什么）
                        CanApi.VCI_ReadErrInfo(
                            _mDeviceType, _DeviceIndex,
                            _CanIndex, ref _CanErrInfo);
                    }
                    else
                    {
                        for (int i = 0; i < _receivedLen; i++)
                        {
                            if (i > _CanRawFrames.Length)
                                break;
                            _CanRawFrame = _CanRawFrames[i];
                            //ID
                            _frameID = string.Format("{0:X8}", _CanRawFrame.ID);
                            //timestamp
                            if (_CanRawFrame.TimeFlag == 0)
                            {
                                _timestamp = "无";
                            }
                            else
                            {
                                _timestamp = string.Format("{0:X}", _CanRawFrame.TimeStamp);
                            }
                            //FrameFormat
                            if (_CanRawFrame.RemoteFlag == 0)
                            {
                                _frameFormat = "数据帧";
                            }
                            else
                            {
                                _frameFormat = "远程帧";
                            }
                            //FrameType
                            if (_CanRawFrame.ExternFlag == 0)
                            {
                                _frameType = "标准帧";
                            }
                            else
                            {
                                _frameType = "扩展帧";
                            }
                            //Data
                            _stringBuilder.Clear();
                            for (int j = 0; j < _CanRawFrame.DataLen; j++)
                            {
                                if (_CanRawFrame.Data.Length <= j) break;
                                _stringBuilder.AppendFormat("{0:X2}", _CanRawFrame.Data[j]);
                            }
                            _frameData = _stringBuilder.ToString();

                            _ReceviedList.Add(new FrameInfo(_frameID, _timestamp, _frameFormat, _frameType, _frameData));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog("CANhelper.ReceiveDataProc." + e.Message);
            }
        }
        #endregion

        #region 引发接收事件
        /// <summary>
        /// 接收到CAN数据帧，引发事件
        /// </summary>
        /// <param name="e"></param>
        void OnFrameInfoArgs(string frameID, string timestamp,
            string frameFormat, string frameType,
            string frameData)
        {
            try
            {
                EventHandler<CanFrameInfoArgs> temp = ReceviedData;
                CanFrameInfoArgs e = new CanFrameInfoArgs(frameID, timestamp, frameFormat, frameType, frameData);
                if (temp != null)
                {
                    temp(this, e);
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog("OnFrameInfoArgs" + e.Message);
            }
        }

        void OnFrameInfoArgs(FrameInfo canframe)
        {
            try
            {
                EventHandler<CanFrameInfoArgs> temp = ReceviedData;
                CanFrameInfoArgs e = new CanFrameInfoArgs(canframe);
                if (temp != null)
                {
                    temp(this, e);
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog("OnFrameInfoArgs" + e.Message);
            }
        }
        #endregion

        #region 设置CAN设备
        public void SetCANdevice(
            uint deviceIndex = 0,
            byte canIndex = 0,
            byte filterType = 0,
            byte canMode = 0,
            string canCode = "00000000",
            string canMask = "FFFFFFFF",
            string canTime0 = "4F",
            string canTime1 = "2F")
        {
            if (_mConnected)
                return;
            _canSetting.SetCAN(
                deviceIndex,
                canIndex,
                filterType,
                canMode,
                canCode,
                canMask,
                canTime0,
                canTime1);
        }
        #endregion

        #region 连接CAN设备
        /// <summary>
        /// 连接CAN设备
        /// </summary>
        public void ConnectCANDevice()
        {
            try
            {
                if (!_mConnected)
                {
                    _openCode = CanApi.VCI_OpenDevice(_mDeviceType, _canSetting.DeviceIndex, 0);    //open device
                    if (_openCode != CanApi.STATUS_OK)
                    {
                        throw new CanException("打开设备失败，VCI_OpenDevice 返回值：" + _openCode);
                    }
                    _CanConfig = _canSetting.GetConfig();
                    _initCode = CanApi.VCI_InitCAN(
                        _mDeviceType,
                        _canSetting.DeviceIndex,
                        _canSetting.CanIndex,
                        ref _CanConfig);    //init device
                    if (_initCode != CanApi.STATUS_OK)
                    {
                        LogHelper.WriteErrorLog("初始化CAN失败，VCI_InitCAN 返回值：" + _initCode);
                        CanApi.VCI_CloseDevice(_mDeviceType, _canSetting.DeviceIndex);
                    }

                    if (_openCode == CanApi.STATUS_OK && _initCode == CanApi.STATUS_OK) _mConnected = true;
                }
            }
            catch (CanException e)
            {
                LogHelper.WriteErrorLog(e.Message);
            }
        }
        #endregion

        #region 复位CAN设备
        /// <summary>
        /// 复位CAN设备
        /// </summary>
        public void ResetCANDevice()
        {
            try
            {
                if (!_mConnected)
                    return;
                if (CanApi.VCI_ResetCAN(_mDeviceType, _canSetting.DeviceIndex,
                    _canSetting.CanIndex) == CanApi.STATUS_OK)
                {
                    return;
                }
                else
                {
                    throw new CanException("复位CAN失败");
                }
            }
            catch (CanException e)
            {
                LogHelper.WriteErrorLog(e.Message);
            }
        }
        #endregion

        #region 启动CAN设备
        /// <summary>
        /// 启动CAN设备
        /// </summary>
        public void StartCAN()
        {
            try
            {
                if (!_mConnected) return;
                if (CanApi.VCI_StartCAN(
                    _mDeviceType, _canSetting.DeviceIndex,
                    _canSetting.CanIndex) != CanApi.STATUS_OK)
                {
                    throw new CanException("启动CAN失败");
                }
                //_ReceviedList.Clear();
                //启动接收线程
                _CanReceviedThread = new Thread(new ThreadStart(ReceiveDataProc));
                _CanReceviedThread.IsBackground = true;
                _CanReceviedThread.Start();
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog(e.Message);
            }

        }
        #endregion

        #region 关闭CAN设备
        public void CloseCANDevice()
        {
            try
            {
                _mConnected = false;
                Thread.Sleep(300); //等待接收线程退出
                CanApi.VCI_CloseDevice(_mDeviceType, _canSetting.DeviceIndex);
            }
            catch (Exception e)
            {
                LogHelper.WriteInfoLog("接收线程退出\n" + e.Message);
            }

        }
        #endregion

        #region 发送
        public bool SendData(string frameID, string timeStamp,
            int frameFormat, int frameType, string frameData,
            int frameSendType)
        {
            if (!_mConnected)
            {
                return false;
            }
            CanApi.VCI_CAN_OBJ[] frameInfo = new CanApi.VCI_CAN_OBJ[1];
            frameInfo = _frameWrapper.Wrapper(frameID, timeStamp, frameFormat,
                frameType, frameData, frameSendType);
            if (
                CanApi.VCI_Transmit(_mDeviceType, _canSetting.DeviceIndex
                , _canSetting.CanIndex, ref frameInfo[0], 1) == CanApi.STATUS_OK)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region 设置过滤器
        /// <summary>
        /// 设置CAN的过滤器
        /// </summary>
        /// <param name="filterType">希望过滤的数据</param>
        public static void SetFilter(string filterType)
        {
            _CanFilter = new CanFilter(filterType);
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public CanHelper()
        {
            _canSetting.ConfigChanged += _canSetting_ConfigChanged;
            //OnReceviedCanInfoDelegate = new OnReceviedCanInfo(OnFrameInfoArgs);
            //OnReceviedCanMsgDelegate = new OnReceviedCanMsg(OnFrameInfoArgs);
            _ReceviedList.CanReceviedEvent += _ReceviedList_CanReceviedEvent;
        }

        void _ReceviedList_CanReceviedEvent(object sender, CanFrameArgs e)
        {
            try
            {
                if (_ReceviedList.IsAvailable)
                {
                    while (_ReceviedList.IsAvailable)
                        OnFrameInfoArgs(_ReceviedList.Remove());
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("_ReceviedList_CanReceviedEvent:" + ex.Message);
            }
        }

        void _canSetting_ConfigChanged(object sender, ConfigChangeArgs e)
        {
            if (_mConnected)
            {
                LogHelper.WriteFatalLog("CAN运行中改变参数");
                CloseCANDevice();
            }
        }

    }
}
