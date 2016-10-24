using System;
using System.Web;
using Common.Models;
using Newtonsoft.Json;

namespace Common.Helper
{
    public class PhotoHelper
    {
        public static string GetRightPhoto(string oldPhoto)
        {
            try
            {
                if (oldPhoto == null) return string.Empty;
                if (oldPhoto.StartsWith("{"))
                {
                    var iconInfo = JsonConvert.DeserializeObject<IconInfo>(oldPhoto);
                    return iconInfo.IconUrl.ToString();
                }
                else
                {
                    if (oldPhoto.ToLower().StartsWith("http"))
                        return oldPhoto;
                    if (HttpContext.Current != null)
                        return new Uri(HttpContext.Current.Request.GetBaseUri(), oldPhoto).ToString();
                    else
                    {
                        var requestContext = RequestContext.GetFromCallContext();
                        if (null != requestContext)
                        {
                            var domain = HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.GetDomain();
                            var baseUri = new Uri(domain);
                            return new Uri(baseUri, oldPhoto).ToString();
                        }
                    }
                    return oldPhoto;
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
