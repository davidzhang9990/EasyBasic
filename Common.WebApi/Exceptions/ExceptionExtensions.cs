using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Exception;
using Newtonsoft.Json.Linq;

namespace Common.WebApi.Exceptions
{
    public static class ExceptionExtensions
    {
        public static HttpError ToHttpError(this StatusException exception, bool shouldIncludeErrorDetail)
        {
            var error = new HttpError(exception.Message);
            foreach (var key in exception.Data.Keys)
            {
                error[key.ToString()] = exception.Data[key].ToString();
            }

            error.Add("Id", Guid.NewGuid());
            error.Add("Time", System.DateTime.UtcNow);
            error.Add("HttpStatusCode", exception.HttpStatusCode);

            if (shouldIncludeErrorDetail)
            {
                error.Add(HttpErrorKeys.ExceptionMessageKey, exception.Message);
                error.Add(HttpErrorKeys.ExceptionTypeKey, exception.GetType().FullName);
                error.Add(HttpErrorKeys.StackTraceKey, exception.StackTrace);
                if (exception.InnerException != null)
                {
                    error.Add(HttpErrorKeys.InnerExceptionKey, new HttpError(exception.InnerException, true));
                }
            }

            return error;
        }

        public static HttpError ToHttpError(this CustomException exception, bool shouldIncludeErrorDetail)
        {
            var error = new HttpError();

            error.Add("Id", Guid.NewGuid());
            error.Add("Time", System.DateTime.UtcNow);
            error.Add("HttpStatusCode", exception.HttpStatusCode);

            dynamic message = JObject.Parse(exception.Message);
            error.Add("Code", message.Code);
            error.Add("Message", message.Message);

            if (shouldIncludeErrorDetail)
            {
                error.Add(HttpErrorKeys.ExceptionMessageKey, exception.Message);
                error.Add(HttpErrorKeys.ExceptionTypeKey, exception.GetType().FullName);
                error.Add(HttpErrorKeys.StackTraceKey, exception.StackTrace);
                if (exception.InnerException != null)
                {
                    error.Add(HttpErrorKeys.InnerExceptionKey, new HttpError(exception.InnerException, true));
                }
            }

            return error;
        }

        public static HttpResponseMessage CreateErrorResponseUsingStatusException(this HttpRequestMessage source, StatusException statusException)
        {
            var shouldIncludeErrorDetail = source.ShouldIncludeErrorDetail();
            var httpError = statusException.ToHttpError(shouldIncludeErrorDetail);
            var response = source.CreateErrorResponse(statusException.HttpStatusCode, httpError);
            return response;
        }

        public static Task<HttpResponseMessage> CreateErrorResponseUsingStatusExceptionAsync(this HttpRequestMessage source, StatusException statusException)
        {
            var response = CreateErrorResponseUsingStatusException(source, statusException);
            var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
            taskCompletionSource.SetResult(response);
            return taskCompletionSource.Task;
        }
    }
}
