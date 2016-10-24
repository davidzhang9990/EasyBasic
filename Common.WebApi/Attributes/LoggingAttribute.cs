using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using log4net;

namespace Common.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class LoggingAttribute : ActionFilterAttribute
    {
        private readonly ILog _log;

        public LoggingAttribute(ILog log)
        {
            _log = log;
        }

        public override void OnActionExecuting(HttpActionContext httpActionContext)
        {
            _log.DebugFormat("{0}.{1}({2})", httpActionContext.ControllerContext.ControllerDescriptor.ControllerName,
                            httpActionContext.ActionDescriptor.ActionName, GetParameters(httpActionContext.ActionDescriptor.GetParameters()));
        }

        public override void OnActionExecuted(HttpActionExecutedContext httpActionExecutedContext)
        {
            if (httpActionExecutedContext.Exception == null)
            {
                _log.DebugFormat("{0}.{1} -> {2}",
                                 httpActionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName,
                                 httpActionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                                 httpActionExecutedContext.Response.GetType().Name);
            }
        }

        public string GetParameters(Collection<HttpParameterDescriptor> parameterDescriptors)
        {
            if (parameterDescriptors.Count.Equals(0))
                return string.Empty;
            var builder = new StringBuilder();
            foreach (var httpParameterDescriptor in parameterDescriptors)
            {
                builder.AppendFormat("{0} {1},", httpParameterDescriptor.ParameterType.Name,
                    httpParameterDescriptor.ParameterName);
            }
            return builder[builder.Length - 1] == ','
                ? builder.Remove(builder.Length - 1, 1).ToString()
                : builder.ToString();
        }
    }
}
