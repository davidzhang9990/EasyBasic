using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using Autofac.Integration.Mvc;
using Common.Ioc;
using Common.Log;

namespace Common.Mvc
{
    public class IocRegistrations : IRegister
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new AutofacWebTypesModule());

            containerBuilder.RegisterType<EqlHttpClient>()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .EnableLog()
                .InstancePerLifetimeScope();
        }
    }
}
