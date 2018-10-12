using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tool450.source.common
{
    /// <summary>
    /// Http助手
    /// 【在最早的http post请求中，只支持application/x-www-form-urlencoded，参数都是通过浏览器的url传递。其实是不支持文件上传的，这样有很多不便。在1995年的时候，出台了rfc1867,也就是《RFC 1867 From-based file upload in HTML》,用以支持文件上传。所以content-type扩充了multipart/form-data用以支持向服务器发送二进制数据。后来随着web应用的增多，增加了诸如application/json的类型。】
    /// </summary>
    public class HttpClientHelper
    {
        private static string getHttpGetParms<T>(T t)
        {
            string parms = string.Empty;
            if (t != null)
            {
                Type type = t.GetType();
                var p = type.GetProperties();
                foreach (var item in p)
                {
                    parms += item.Name + "=" + item.GetValue(t).ToString() + "&";

                }
                parms = "?" + parms.TrimEnd('&');
            }
            return parms;
        }
        private static Dictionary<string, string> getHttpPostParms<T>(T t)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (t != null)
            {
                Type type = t.GetType();
                var p = type.GetProperties();
                foreach (var item in p)
                {
                    dic.Add(item.Name, item.GetValue(t).ToString());
                }
            }
            return dic;
        }
        public static string Get(string url, int tOut = 1000 * 60 * 5)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var response = new HttpClient().GetAsync(url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
            }).Wait(tOut);
            return result;
        }
        public static string Get<T>(string url, T t, int tOut = 1000 * 60 * 5)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    url = url + getHttpGetParms(t);
                    using (var response = new HttpClient().GetAsync(url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
            }).Wait(tOut);
            return result;
        }

        public static string Post(string url, int tOut = 1000 * 60 * 5)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>());
                    using (var response = new HttpClient().PostAsync(url, content).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
            }).Wait(tOut);
            return result;
        }

        public static string Post<T>(string url, T t, int tOut = 1000 * 60 * 5)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var content = new FormUrlEncodedContent(getHttpPostParms(t));
                    content.Headers.ContentType.MediaType = "application/json";
                    using (var response = new HttpClient().PostAsync(url, content).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
            }).Wait(tOut);
            return result;
        }

        /// <summary>
        /// 以ContentType = "application/x-www-form-urlencoded"形式发送Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="t"></param>
        /// <param name="tOut"></param>
        /// <returns></returns>
        public static string PostToUrlencoded<T>(string url, T t, int tOut = 1000 * 60 * 5)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                    var content = new StringContent(json, Encoding.UTF8);
                    content.Headers.ContentType.MediaType = "application/json";
                    using (var response = new HttpClient().PostAsync(url, content).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
            }).Wait(tOut);
            return result;
        }

        /// <summary>
        /// 以ContentType = "application/json"形式发送Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="t"></param>
        /// <param name="tOut"></param>
        /// <returns></returns>
        public static string PostToJson<T>(string url, T t, int tOut = 1000 * 60 * 5)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                    var content = new StringContent(json, Encoding.UTF8);
                    content.Headers.ContentType.MediaType = "application/json";
                    using (var response = new HttpClient().PostAsync(url, content).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
            }).Wait(tOut);
            return result;
        }

        /// <summary>
        /// 以ContentType = "multipart/form-data"形式发送Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="t"></param>
        /// <param name="tOut"></param>
        /// <returns></returns>
        public static string PostToFormData<T>(string url, T t, int tOut = 1000 * 60 * 5)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var form = new MultipartFormDataContent())
                    {
                        PropertyInfo[] ps = t.GetType().GetProperties();
                        foreach (PropertyInfo p in ps)
                        {
                            if (p.PropertyType == typeof(string))//属性的类型判断  
                            {
                                form.Add(new StringContent((string)p.GetValue(t)), $"\"{p.Name}\"");
                            }
                            if (p.PropertyType == typeof(byte[]))
                            {
                                var imageContent = new ByteArrayContent((byte[])p.GetValue(t));
                                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                                imageContent.Headers.Add("Content-Type", "multipart/form-data");
                                imageContent.Headers.Add("Content-Disposition", $"form-data; name=\"{p.Name}\"; filename=\"{fileName}\"");
                                form.Add(imageContent, p.Name, fileName);
                            }
                            //foreach (var keyValuePair in dic)
                            //{
                            //    if (keyValuePair.Value != null)
                            //    {
                            //        form.Add(new StringContent(keyValuePair.Value.ToString()), String.Format("\"{0}\"", keyValuePair.Key));
                            //    }
                            //}
                        }
                        using (var response = new HttpClient().PostAsync(url, form).Result)
                        {
                            response.EnsureSuccessStatusCode();
                            result = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
            }).Wait(tOut);
            return result;
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="name">文件名(含后缀)</param>
        /// <param name="path">路径</param>
        /// <returns>运行结果</returns>
        public static bool FileDownload(string url, string name, string path, int tOut = 1000 * 60 * 5)
        {
            bool value = false;
            if (string.IsNullOrEmpty(url)) return false;
            if (string.IsNullOrEmpty(name)) return false;
            if (string.IsNullOrEmpty(path)) return false;
            
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    if (!path.EndsWith("\\")) path += "\\";
                    string fullpath = path + name;
                    if (File.Exists(fullpath)) File.Delete(fullpath);
                    using (var response = new HttpClient().GetAsync(url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;
                        using (FileStream fStream = new FileStream(fullpath, FileMode.CreateNew))
                        {
                            using (BufferedStream bStream = new BufferedStream(fStream))
                            {
                                bStream.Write(bytes, 0, bytes.Length);
                                bStream.Flush();
                            }
                        }
                    }
                    value = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                    value = false;
                }
            }).Wait(tOut);
            return value;
        }

    }
}
