using XiaLM.MotionDetector.EventArg;

namespace XiaLM.MotionDetector.VideoSource
{
    public interface IVideoSource
    {
        event CameraEventHandler NewFrame;

        /// <summary>
		/// 视频源地址(文件路径/URL等等)
		/// </summary>
		string VideoSource { get; set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        object UserData { get; set; }

        /// <summary>
        /// 获取从视频源接收到的帧数
        /// </summary>
        int FramesReceived { get; }

        /// <summary>
        /// 获取从视频源接收到的字节数
        /// </summary>
        int BytesReceived { get; }
        
        /// <summary>
        /// 视频源是否是运行状态
        /// </summary>
        bool Running { get; }

        /// <summary>
        /// 开始视频源
        /// </summary>
        void Start();

        /// <summary>
        /// 停止视频源
        /// </summary>
        void Stop();
    }
}
