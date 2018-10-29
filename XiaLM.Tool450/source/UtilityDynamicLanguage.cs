using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tool450.source
{
    /// <summary>
    /// 动态语言帮助类
    /// </summary>
    public class UtilityDynamicLanguage
    {
        public void play()
        {
            //动态语言（IronPython）执行类，可于解析和执行动态语言代码。
            ScriptEngine engine = Python.CreateEngine();
            //ScriptScope：构建一个执行上下文，其中保存了环境及全局变量；宿主(Host)可以通过创建不同的 ScriptScope 来提供多个数据隔离的执行上下文。
            ScriptScope scope = engine.CreateScope();
            //操控动态语言代码的类型，可以编译（Compile）、运行（Execute）代码。
            ScriptSource script = engine.CreateScriptSourceFromFile(@"D:\004WorkSpaces\PyCharm_WorkSpaces\pyaudio\audio\_init_.py");
            var result = script.Execute(scope);
        }
    }
}
