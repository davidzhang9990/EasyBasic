using System;
using System.Linq;
using System.Web;

namespace Common.Mvc
{
    public static class ViewHelper
    {
        //public static void SetStyleForCurrentView(this HtmlHelper helper,string viewName)
        //{
        //    if (helper.ViewContext.RouteData.Values["action"].ToString() == viewName)
        //    {
        //        helper.
        //    }
        //}

        public static bool CheckAgent(string key)
        {
            var agent = HttpContext.Current.Request.UserAgent;
            if (agent == null || (agent.Contains("Windows NT") || agent.Contains("Macintosh"))) return false;
            //return  agent.Contains(key);
            return agent.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0;
        }
        public static bool CheckMobileAgent()
        {
            var agent = HttpContext.Current.Request.UserAgent;
            var keywords = new[] { "Android", "iPhone", "iPod", "iPad", "Windows Phone", "MQQBrowser" };
            if (agent == null || (agent.Contains("Windows NT") || agent.Contains("Macintosh"))) return false;
            return keywords.Any(CheckAgent);
        }
    }
}
