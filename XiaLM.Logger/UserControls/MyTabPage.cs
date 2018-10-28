using System.Windows.Forms;

namespace XiaLM.Logger.UserControls
{
    public partial class MyTabPage : UserControl
    {
        /// <summary>
        /// 是否刷新文本
        /// </summary>
        public bool IsRefreshTetx = true;
        public MyTabPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 文本变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_TextChanged(object sender, System.EventArgs e)
        {
            if (this.richTextBox1.Lines.Length > 500)   //显示不超过500行
            {
                int firstLenth = this.richTextBox1.Lines[0].ToCharArray().Length;
                this.richTextBox1.Text.Remove(0, firstLenth);
            }
        }
    }
}
