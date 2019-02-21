using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.MotionDetector.MotionDetector
{
    /// <summary>
    /// 运动检测接口
    /// </summary>
    public interface IMotionDetector
    {
        /// <summary>
		/// 是否是水平运动
		/// </summary>
		bool MotionLevelCalculation { set; get; }

        /// <summary>
        /// 水平运动的百分比变化量
        /// </summary>
        double MotionLevel { get; }

        /// <summary>
        /// 处理当前画面返回新的画面
        /// </summary>
        void ProcessFrame(ref Bitmap image);

        /// <summary>
        /// 复位检测器初始状态
        /// </summary>
        void Reset();
    }
}
