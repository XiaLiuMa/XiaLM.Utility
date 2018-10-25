using System;

namespace XiaLM.Can
{
    public class CanFrameArgs : EventArgs
    {
        FrameInfo _canFrame = new FrameInfo("1", "1", "1", "1", "1");

        public FrameInfo CanFrame
        {
            get { return _canFrame; }
            set { _canFrame = value; }
        }

        public CanFrameArgs() { }

        public CanFrameArgs(FrameInfo frame)
        {
            _canFrame = frame;
        }

    }
}
