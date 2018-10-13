using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaLM.Ftp.Model;
using XiaLM.Tool450.source.common;

namespace XiaLM.Ftp
{
    public class FtpClient
    {
        public string ip { get; set; }    //ftp服务器ip
        public string uName { get; set; }    //用户名
        public string pWord { get; set; }    //密码
        private BlockingCollection<DownloadFile> downloadBlocking;   //下载队列【阻塞集合】
        private IObservable<ProgressBarInfo> progressObservable;    //可观察序列
        private event Action<ProgressBarInfo> progressBarEvent = (s) => { };
        private static readonly object lockObj = new object();
        private static FtpClient instance;
        public FtpClient()
        {
            ip = "192.168.1.247";
            uName = "XLMftp";
            pWord = "666666";
            downloadBlocking = new BlockingCollection<DownloadFile>();
            progressObservable = Observable.FromEvent<ProgressBarInfo>(p => this.progressBarEvent += p, p => this.progressBarEvent -= p);
            progressObservable.Sample(TimeSpan.FromSeconds(1)).Subscribe(r => { ProgressBarEvent(r.Session, r.Progress); });

            AutoDownloadFiles();
        }
        public static FtpClient GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new FtpClient();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 获取FTP请求对象
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <returns></returns>
        public FtpWebRequest GetRequest(string url)
        {
            FtpWebRequest result = (FtpWebRequest)FtpWebRequest.Create(url);
            result.Credentials = new NetworkCredential(uName, pWord);    //提供身份验证信息
            result.KeepAlive = false;   //请求完成后是否保持服务器连接控制，默认为true
            return result;
        }

        /// <summary>
        /// 检索服务器文件
        /// </summary>
        /// <param name="targetDir"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<FtpFile> RetrieveServerFiles(string targetDir, string pattern)
        {
            List<FtpFile> result = new List<FtpFile>();
            try
            {
                string url = "ftp://" + ip + "/" + targetDir + "/" + pattern;
                FtpWebRequest ftp = GetRequest(url);
                ftp.Method = WebRequestMethods.Ftp.ListDirectory;
                ftp.UsePassive = true;
                ftp.UseBinary = true;

                string responseStr = string.Empty;
                using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
                {
                    long size = response.ContentLength;
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(dataStream, Encoding.UTF8))
                        {
                            responseStr = sr.ReadToEnd();
                            sr.Close();
                        }
                        dataStream.Close();
                    }
                    response.Close();
                }
                responseStr = responseStr.Replace("\r\n", "\r").TrimEnd('\r');
                responseStr = responseStr.Replace("\n", "\r");
                if (!string.IsNullOrEmpty(responseStr))
                {
                    var strs = responseStr.Split('\r');
                    if (strs != null && strs.Length > 0)
                    {
                        foreach (string str in strs)
                        {
                            FtpFile ftpFile = new FtpFile();
                            ftpFile.Name = Path.GetFileNameWithoutExtension(str);
                            ftpFile.FullName = str;
                            ftpFile.Path = "ftp://" + ip + "/" + targetDir + "/" + str;

                            #region 获取ftp文件大小
                            long fileSize = 0;  //文件大小
                            FtpWebRequest reqFtp = GetRequest(ftpFile.Path);
                            reqFtp.UseBinary = true;
                            reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;
                            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
                            {
                                fileSize = response.ContentLength;
                                response.Close();
                            }
                            #endregion

                            ftpFile.Size = fileSize;
                            result.Add(ftpFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
            return result;
        }

        /// <summary>
        /// 添加下载文件
        /// </summary>
        /// <param name="localDir">下载至本地的路径</param>
        /// <param name="files">要下载的文件集合</param>
        public void AddDownloadFiles(string localDir, List<FtpFile> files)
        {
            try
            {
                if (files.Count <= 0) return;
                foreach (FtpFile item in files)
                {
                    string localFile = localDir + @"\" + item.FullName;
                    downloadBlocking.TryAdd(new DownloadFile() { FtpFile = item, SavePath = localFile });
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 自动下载文件，新开启一个线程专门负责下载
        /// </summary>
        public void AutoDownloadFiles()
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var item in downloadBlocking.GetConsumingEnumerable())
                {
                    FtpWebRequest ftp = GetRequest(item.FtpFile.Path);
                    ftp.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftp.UseBinary = true;
                    ftp.UsePassive = false;
                    using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            using (FileStream fs = new FileStream(item.SavePath, FileMode.CreateNew))
                            {
                                try
                                {
                                    byte[] bytes = new byte[1024 * 1024 * 5];
                                    int readCount = 0;
                                    double percent = 0; //大小百分比
                                    do
                                    {
                                        readCount = dataStream.Read(bytes, 0, bytes.Length);
                                        fs.Write(bytes, 0, readCount);
                                        if (item.FtpFile.Size <= 0) continue;   //大小为0，跳过当前循环，继续下一次循环。
                                        percent = (double)fs.Length / (double)item.FtpFile.Size * 100;
                                        progressBarEvent(new ProgressBarInfo() { Session = item.FtpFile.Name, Progress = percent });
                                    }
                                    while (readCount > 0);
                                    fs.Flush();
                                    fs.Close();
                                }
                                catch (Exception ex)
                                {
                                    fs.Close();
                                    File.Delete(item.SavePath);
                                }
                            }
                            dataStream.Close();
                        }
                        response.Close();
                    }
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ftpFile">需要上传的文件</param>
        /// <param name="targetDir">目标路径</param>
        public void UploadFile(FtpFile ftpFile, string targetDir)
        {
            if (string.IsNullOrEmpty(targetDir)) return;

            //string target = string.Empty;
            //target = Guid.NewGuid().ToString(); //使用临时文件名
            //string url = "FTP://" + hName + "/" + targetDir + "/" + target;
            string url = "ftp://" + ip + "/" + targetDir + "/" + ftpFile.Name;

            FtpWebRequest ftp = GetRequest(url);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="progress"></param>
        public void ProgressBarEvent(string fileName, double progress)
        {

        }
    }
}
