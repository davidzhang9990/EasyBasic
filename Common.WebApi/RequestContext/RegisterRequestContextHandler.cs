using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.WebApi;
using Common.Helper;
using Common.Models;

namespace Common.WebApi.RequestContext
{
    public class RegisterRequestContextHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var scope = request.GetDependencyScope();

            var requestContext = GetRequestContext(request);

            var requestScope = scope.GetRequestLifetimeScope();
            if (requestScope != null)
            {
                var registry = requestScope.ComponentRegistry;
                var builder = new ContainerBuilder();
                builder.Register(context => requestContext).InstancePerLifetimeScope();
                builder.Update(registry);
            }

            Models.RequestContext.SetToCallContext(requestContext);

            request.Properties[Models.RequestContext.Key] = requestContext;

            return base.SendAsync(request, cancellationToken);
        }

        private static Models.RequestContext GetRequestContext(HttpRequestMessage request)
        {
            if (RequestContextExtensions.GetRequestContext(request) != null)
            {
                return RequestContextExtensions.GetRequestContext(request);
            }
            else
            {
                var requestContext = new Models.RequestContext();
                var userId = ClaimHelper.GetClaimValue(EqlClaimType.UserId);
                var schoolId = ClaimHelper.GetClaimValue(EqlClaimType.SchoolId);
                var roles = ClaimHelper.GetClaimValue(EqlClaimType.Roles);
                requestContext.UserId = string.IsNullOrEmpty(userId) ? Guid.Empty : new Guid(userId);
                if (string.IsNullOrEmpty(roles))
                    requestContext.Roles = new List<EnumRole>();
                else
                {
                    var roleArray = roles.Split(',');
                    requestContext.Roles =
                        roleArray.Select(x => (EnumRole)Enum.Parse(typeof(EnumRole), x.ToString(), true)).ToList();
                }
                return requestContext;
            }
        }
    }
}
