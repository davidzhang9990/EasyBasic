using System.Net.Http;

namespace Common.WebApi.RequestContext
{
    public static class RequestContextExtensions
    {
        public static Models.RequestContext GetRequestContext(HttpRequestMessage httpRequestMessage)
        {
            if (!httpRequestMessage.Properties.ContainsKey(Models.RequestContext.Key))
            {
                return null;
            }

            var requestContext = httpRequestMessage.Properties[Models.RequestContext.Key] as Models.RequestContext;
            return requestContext;
        }
    }
}
