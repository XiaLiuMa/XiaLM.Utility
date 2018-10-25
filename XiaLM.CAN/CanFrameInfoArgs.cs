using System;
using XiaLM.Can.Help;

namespace XiaLM.Can
{
    public class CanFrameInfoArgs : EventArgs
    {
        /// <summary>
        /// CAN 的接收帧
        /// </summary>
        FrameInfo _canFrameInfo = new FrameInfo("1", "1", "1", "1", "1");
        internal FrameInfo CanFrameInfo
        {
            get { return _canFrameInfo; }
        }

        public CanFrameInfoArgs(string frameID, string timeStamp, string frameFormat, string frameType, string data)
        {
            try
            {
                if (frameID.Equals(string.Empty) &&
                timeStamp.Equals(string.Empty) &&
                frameFormat.Equals(string.Empty) &&
                frameType.Equals(string.Empty) &&
                data.Equals(String.Empty))
                {
                    throw new CanException("CANFrameInfo 参数为空");
                }
                else
                {
                    _canFrameInfo.FrameID = frameID;
                    _canFrameInfo.TimeStamp = timeStamp;
                    _canFrameInfo.FrameFormat = frameFormat;
                    _canFrameInfo.FrameType = frameType;
                    _canFrameInfo.Data = data;
                }
            }
            catch (CanException e)
            {
                LogHelper.WriteErrorLog(e.Message);
            }
        }

        public CanFrameInfoArgs(FrameInfo frame)
        {
            try
            {
                if (frame == null)
                {
                    throw new CanException("CANFrameInfoArgs 参数不能为空");
                }
                _canFrameInfo = frame;
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog(e.Message);
            }
        }
    }
}
