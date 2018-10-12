using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace XiaLM.Tool450.source.common
{
    /// <summary>
    /// 文件读写工具
    /// </summary>
    public class FileReadWriteHelper
    {
        /// <summary>
        /// 读取文件到byte[]
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static byte[] ReadBytesFromFile(string imagePath)
        {
            byte[] byteData;
            using (FileStream fs = new FileStream(imagePath, FileMode.Open))
            {
                byteData = new byte[fs.Length];
                fs.Read(byteData, 0, byteData.Length);
                fs.Close();
            }
            return byteData;
        }

        /// <summary>
        /// 写入byte[]到文件
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="imgBytes"></param>
        public static void WriteBytesToFile(string imagePath, byte[] imgBytes)
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Create))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(imgBytes, 0, imgBytes.Length);
                fs.Flush();
                fs.Close();
            }
        }

        /// <summary>
        /// 读取Bitmap位图到字节数组
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] ReadBytesFromBitmap(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }

        /// <summary>
        /// 写入byte[]到Bitmap位图
        /// </summary>
        /// <param name="bmpBytes"></param>
        /// <param name="imageSize"></param>
        /// <returns></returns>
        private static Bitmap WriteBytesToBitmap(byte[] bmpBytes, Size imageSize)
        {
            Bitmap bmp = new Bitmap(imageSize.Width, imageSize.Height);
            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, imageSize.Width, imageSize.Height),
            ImageLockMode.ReadWrite,
            PixelFormat.Format24bppRgb);
            Marshal.Copy(bmpBytes, 0, bData.Scan0, bmpBytes.Length);
            bmp.UnlockBits(bData);
            return bmp;
        }

    }
}
