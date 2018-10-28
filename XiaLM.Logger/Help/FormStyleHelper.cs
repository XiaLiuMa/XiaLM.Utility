using System;
using System.Drawing;
using System.Windows.Forms;

namespace XiaLM.Logger.Help
{
    public class FormStyleHelper
    {
        /// <summary>
        /// 窗体改变尺寸
        /// </summary>
        /// <param name="control"></param>
        /// <param name="defaleSize"></param>
        /// <param name="newSize"></param>
        public static void FormChangeSize(Control control, Size defaleSize, Size newSize)
        {
            double heightProportion = (double)newSize.Height / defaleSize.Height;  //高度比例
            double widthProportion = (double)newSize.Width / defaleSize.Width;  //宽度比例
            foreach (Control item in control.Controls)
            {
                item.Size = new Size()  //设置尺寸
                {
                    Height = (int)(item.Size.Height * heightProportion + 0.5),
                    Width = (int)(item.Size.Width * widthProportion + 0.5)
                };
                item.Location = new Point() //设置左上角坐标
                {
                    X = (int)(item.Location.X * widthProportion + 0.5),
                    Y = (int)(item.Location.Y * heightProportion + 0.5)
                };
                if (item.Controls.Count > 0) FormChangeSize(item, defaleSize, newSize); //迭代控件
            }
        }

        /// <summary>
        /// 设置光标到文本编辑框指定位置并选中指定长度
        /// </summary>
        /// <param name="tBox">文本框</param>
        /// <param name="index">光标其实位置</param>
        /// <param name="lenth">选中长度</param>
        public static void SetCursorToTextBoxBase(TextBoxBase tBox, int index, int lenth)
        {
            tBox.Focus();//让文本框获取焦点
            tBox.Select(index, lenth);//设置光标选中的文本
            tBox.ScrollToCaret();//滚动到控件光标处
        }

        /// <summary>
        /// RichTextBox中指定文本所在行设置高亮
        /// </summary>
        /// <param name="text"></param>
        public static void SetHighlightContent(RichTextBox tBox, string text, Color color)
        {
            int start = 0;  //起始位置
            int end = tBox.Text.Length; //结束位置
            int index = tBox.Find(text, start, end, RichTextBoxFinds.None);
            while (index != -1)
            {
                tBox.SelectionFont = new Font(tBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);
                tBox.SelectionColor = color;
                start = index + text.Length;
                index = tBox.Find(text, start, end, RichTextBoxFinds.None);
            }
        }
    }
}
