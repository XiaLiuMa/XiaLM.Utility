using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace XiaLM.Owin.source.UserAttributes
{
    public class ControllerAttribute : Attribute, IHttpController
    {
        public Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
