using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tool450.source.common
{
    /// <summary>
    /// 项目资源管理助手
    /// </summary>
    public class ResourceHelper
    {
        /// <summary>
        /// 获取资源文件中的所有内容
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        private static PropertyInfo[] GetPropertys(object resource)
        {
            return resource.GetType().GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// 获取资源文件中指定名称的属性
        /// (用已知类型去接收就可以了，如：Bitmap b = object as Bitmap;)
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="pName"></param>
        /// <returns></returns>
        private static object GetProperty(object resource,string pName)
        {
            var propertyInfos = resource.GetType().GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo item in propertyInfos)
            {
                if (item.Name.Equals(pName))
                {
                    return item.GetValue(resource, null);
                }
            }
            return null;
        }

        
    }
}
