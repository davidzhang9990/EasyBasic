using System;
using System.Reflection;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using XDDEasy.WebApi.Host;
using Common.Ioc;
using Common.WebApi.Handler;
using Configuration = Common.WebApi.Configuration;

namespace XDDEasy.Main
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //regist web api
            var container = ConfigureWebApi(GlobalConfiguration.Configuration);
            //set mvc dependen
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            ConfigureMvcMapper();
            AreaRegistration.RegisterAllAreas();
            Common.Mvc.Configuration.Configure();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        public static IContainer ConfigureWebApi(HttpConfiguration httpConfiguration)
        {
            WebApi.Host.MapperConfig.Configure();
            AttributeRoutingHttpConfig.RegisterRoutes(httpConfiguration);
            var container = Configuration.Configure(httpConfiguration);
            return container;
        }

        public static void ConfigureMvcMapper()
        {
            //Eql.WebSite.MapperConfig.Configure();
            //Eql.WebSite.Areas.Teacher.MapperConfig.Configure();
        }
    }
}
