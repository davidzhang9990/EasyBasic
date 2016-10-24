using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using XDDEasy.Contract.LogContract;
using XDDEasy.Domain.LogAggregates;
using Common.EntityFramework.DataAccess;

namespace Common.WebApi
{
    public class OperatorLogHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var container = ((AutofacWebApiDependencyResolver)GlobalConfiguration.Configuration.DependencyResolver).Container;
            var logRespository = container.Resolve<ILogRepository>();

            var attributedRoutesData = request.GetRouteData().GetSubRoutes();
            var subRouteData = attributedRoutesData.FirstOrDefault();
            if (subRouteData != null)
            {
                var actions = (HttpActionDescriptor[])subRouteData.Route.DataTokens["actions"];
                var controllerName = actions[0].ControllerDescriptor.ControllerName;
                var actionName = actions[0].ActionName;
                if (actionName != "GetRolePages")
                {
                    var property = string.Format("{0} {1}", controllerName, actionName);
                    logRespository.SaveLog(new CreateLogRequest()
                    {
                        Content = request.RequestUri.PathAndQuery,
                        Property = property,
                        Type = 3
                    });
                    if (logRespository.UnitOfWork != null && !logRespository.UnitOfWork.IsInTransaction)
                        logRespository.UnitOfWork.SaveChanges();
                }
            }

            return base.SendAsync(request, cancellationToken);


            /*return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                var container = ((AutofacWebApiDependencyResolver)GlobalConfiguration.Configuration.DependencyResolver).Container;
                var logRespository = container.Resolve<ILogRepository>();

                //request.Properties["MS_HttpRoute_Data"]
                var route = request.GetRouteData().Route;

                var actions = (HttpActionDescriptor[])route.DataTokens["actions"];
                var controllerName = actions[0].ControllerDescriptor.ControllerName;
                var actionName = actions[0].ActionName;
                if (actionName != "GetRolePages")
                {
                    var property = string.Format("{0} {1}", controllerName, actionName);
                    logRespository.SaveLog(new CreateLogRequest()
                    {
                        Content = request.RequestUri.PathAndQuery,
                        Property = property,
                        Type = 3
                    });
                    logRespository.UnitOfWork.SaveChanges();
                }

                return task.Result;
            }, cancellationToken);*/
        }
    }
}
