using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using XDDEasy.Contract.UserLog;
using XDDEasy.Domain.UserLogger;
using Common.Helper;
using Common.Log;
using Common.Models;
using log4net;

namespace Common.WebApi.Handler
{
    public class UserLogHandler : DelegatingHandler
    {
        private readonly IUserLogServices _iUserLogServices;
        private readonly ILog _log;

        public UserLogHandler(IContainer iContainer)
        {
            _iUserLogServices = iContainer.Resolve<IUserLogServices>();
            _log = iContainer.Resolve<ILog>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                string requestType = request.Method.ToString().ToUpper();

                if (requestType == "POST" || requestType == "DELETE" || requestType == "PUT")
                {
                    var user = await GetUserLogResponse(request);
                    _iUserLogServices.Add(user);
                }
            }
            catch (System.Exception exception)
            {
                _log.Exception(exception);
            }
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            return response;

        }

        private async Task<UserLogResponse> GetUserLogResponse(HttpRequestMessage request)
        {
            HttpRequestBase baseRequest = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request;

            var userLogResponse = new UserLogResponse()
            {
                UserId = ClaimHelper.GetClaimValue(EasyClaimType.UserId),
                Ip = baseRequest[baseRequest.ServerVariables["HTTP_VIA"] != null
                                ? "HTTP_X_FORWARDED_FOR"
                                : "REMOTE_ADDR"],
                //baseRequest[baseRequest.ServerVariables["HTTP_VIA"] != null
                //            ? "HTTP_X_FORWARDED_FOR"
                //            : "REMOTE_ADDR"],
                UserAgent = baseRequest.UserAgent,
                Host = baseRequest.GetDomain(),
                Path = baseRequest.Path
            };
            userLogResponse.Content = await request.Content.ReadAsStringAsync();
            return userLogResponse;
        }
    }
}
