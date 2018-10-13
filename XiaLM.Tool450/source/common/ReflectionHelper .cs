using System;
using System.Collections.Generic;
using System.Reflection;

namespace XiaLM.Tool450.source.common
{
    /// <summary>
    /// 反射助手
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// 反射获取属性字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetProperties<T>(T t) where T : class  // 这是参数类型约束，指定T必须是Class类型。        
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Type type = t.GetType();//获得该类的Type                                          
            foreach (PropertyInfo pi in type.GetProperties())   //再用Type.GetProperties获得PropertyInfo[],然后就可以用foreach 遍历了
            {
                object value1 = pi.GetValue(t, null);//用pi.GetValue获得值                
                string name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作                                  
                dic.Add(name, value1);
            }
            return dic;
        }

    }
}
