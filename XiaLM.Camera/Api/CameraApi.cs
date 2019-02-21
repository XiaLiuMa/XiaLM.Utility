using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Camera.Api
{
    public class CameraApi
    {
        [DllImport("avicap32.dll ")]
        public static extern IntPtr capCreateCaptureWindowA(byte[] lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);

        /*新加的一个API调用，但是在编译的时候出错，提示“无法在 DLL“avicap32.dll ”中找到名为“capCreateCaptureWindowB”的入口点。”*/
        [DllImport("avicap32.dll ")]
        public static extern IntPtr capCreateCaptureWindowB(byte[] lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);

        [DllImport("avicap32.dll ")]
        public static extern int capGetVideoFormat(IntPtr hWnd, IntPtr psVideoFormat, int wSize);

        /// <summary>
        /// 这里特别注意，因为WinAPI中的long为32位，而C#中的long为64wei，所以需要将lParam该为int
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll ")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        //[DllImport("avicap32.dll ")]//////add a window
        //private static extern IntPtr capCreateCaptureWindowB(byte[] lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID); 


    }
}
