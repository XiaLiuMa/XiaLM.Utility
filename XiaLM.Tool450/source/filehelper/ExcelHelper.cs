using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace XiaLM.Tool450.source.filehelper
{
    /// <summary>
    /// Execl操作帮助类
    /// </summary>
    public class ExcelHelper
    {
        private static readonly object lockObj = new object();
        private static ExcelHelper instance;
        public static ExcelHelper GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new ExcelHelper();
                    }
                }
            }
            return instance;
        }

        public ExcelHelper()
        {

        }

        /// <summary>
        /// 打开一个Excel文件
        /// </summary>
        /// <param name="fileName">excel文件名，包括文件路径</param>
        public IWorkbook OpenExecl(string fileName)
        {
            IWorkbook workbook = null;
            using (FileStream fs = File.OpenRead(fileName))
            {
                //判断文件格式:HSSF只能读取xls,XSSF只能读取xlsx格式的
                if (Path.GetExtension(fs.Name) == ".xls")
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else if (Path.GetExtension(fs.Name) == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fs);
                }
            }

            return workbook;
        }
    }
}
