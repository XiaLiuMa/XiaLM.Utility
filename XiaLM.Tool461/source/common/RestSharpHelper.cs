using RestSharp;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace XiaLM.Tool461.source.common
{
    /// <summary>
    /// RestSharp框架的Http请求助手
    /// </summary>
    public class RestSharpHelper
    {
        public static string PostMultipartFormData<T>(string url, T t, int tOut = 1000 * 60 * 5)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    RestClient client = new RestClient(url);
                    RestRequest request = new RestRequest(Method.POST);
                    request.RequestFormat = DataFormat.Json;
                    PropertyInfo[] ps = t.GetType().GetProperties();
                    foreach (PropertyInfo p in ps)
                    {
                        if (p.PropertyType == typeof(string))//属性的类型判断  
                        {
                            request.AddParameter(p.Name, (string)p.GetValue(t));
                        }
                        if (p.PropertyType == typeof(byte[]))
                        {
                            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                            request.AddFile(p.Name, (byte[])p.GetValue(t), fileName, contentType: "application/x-img");//非压缩格式 不设置为contentType 则默认压缩格式
                        }
                    }
                    result = client.Execute(request).Content;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
            }).Wait(tOut);
            return result;
        }
    }
}
