using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common.Helper;

namespace Common.Mvc.Filters
{
    public class CrosAttribute : ActionFilterAttribute
    {
        private const string Origin = "Origin";
        private const string AccessControlRequestMethod = "Access-Control-Request-Method";
        private const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        private const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";
        private const string AccessControlExposeHeaders = "Access-Control-Expose-Headers";
        private const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var originHeaders = filterContext.HttpContext.Request.Headers.GetValues(Origin);
            var isCorsRequest = originHeaders != null && originHeaders.Length > 0;
            var credentials = filterContext.HttpContext.Response.Headers[AccessControlAllowCredentials];
            if (!string.IsNullOrEmpty(credentials) || isCorsRequest)
            {
                var isCredentials = false;
                bool.TryParse(credentials, out isCredentials);
                if (isCredentials)
                {
                    filterContext.HttpContext.Response.Headers.Remove(AccessControlAllowOrigin);
                    filterContext.HttpContext.Response.Headers.Add(AccessControlAllowOrigin, filterContext.HttpContext.Request.Headers[Origin]);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
