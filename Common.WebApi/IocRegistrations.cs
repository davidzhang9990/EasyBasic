using System.Linq;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Common.Ioc;
using Common.WebApi.Exceptions;

namespace Common.WebApi
{
    public class IocRegistrations : IRegister
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            RegisterWebApi(containerBuilder);
        }

        public static void RegisterWebApi(ContainerBuilder containerBuilder)
        {
            var assemblies = AssemblyHelper.Assemblies;
            containerBuilder.RegisterApiControllers(assemblies.ToArray());
            containerBuilder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            containerBuilder.RegisterType<CommonExceptionFilterAttribute>();
        }

    }
}
