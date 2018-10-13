using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Ftp.Model
{
    public class DownloadFile
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        public FtpFile FtpFile { get; set; }
        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath { get; set; }
    }
}
