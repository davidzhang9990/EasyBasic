using System.Web.Http;

namespace XDDEasy.WebApi.Host
{
    public static class AttributeRoutingHttpConfig
    {
        public static void RegisterRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
    }
}
