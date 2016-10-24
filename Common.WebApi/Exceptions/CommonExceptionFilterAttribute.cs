using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.Http.Filters;
using XDDEasy.Domain.ResourceAggregates;
using Common.Exception;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.WebApi.Exceptions
{
    public class CommonExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILog _log;

        public CommonExceptionFilterAttribute(ILog log)
        {
            _log = log;
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var shouldIncludeErrorDetail = actionExecutedContext.Request.ShouldIncludeErrorDetail();
            HttpError error = null;
            var statusCode = HttpStatusCode.BadRequest;

            if (actionExecutedContext.Exception.GetType() == typeof(CustomException))
            {
                var customException = actionExecutedContext.Exception as CustomException;
                if (customException != null)
                {
                    error = customException.ToHttpError(shouldIncludeErrorDetail);
                    statusCode = customException.HttpStatusCode;
                }
            }
            else
            {
                var statusException = actionExecutedContext.Exception as StatusException ??
                                      new InternalServerErrorException(
                                          EasyResource.Value("Exception_InternalServerError"),
                                          actionExecutedContext.Exception);

                error = statusException.ToHttpError(shouldIncludeErrorDetail);
                statusCode = statusException.HttpStatusCode;
            }

            if (error == null) return;

            actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(statusCode, error);
            _log.Error(error.Message, actionExecutedContext.Exception);
        }
    }
}
