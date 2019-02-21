using System;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace XiaLM.Owin.source.UserAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public  class HttpPostAttribute : Attribute, System.Web.Http.Controllers.IActionHttpMethodProvider
    {
        System.Web.Http.HttpPostAttribute httpAttribute;
        public HttpPostAttribute()
        {
            httpAttribute = new System.Web.Http.HttpPostAttribute();
        }
        public Collection<HttpMethod> HttpMethods => httpAttribute.HttpMethods;
    }
}
