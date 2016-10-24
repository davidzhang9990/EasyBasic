using Autofac;
using Common.Ioc;
using log4net;

namespace Common.Log
{
    public class IocRegistrations : IRegister
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            log4net.Config.XmlConfigurator.Configure();

            containerBuilder.Register(x => LogManager.GetLogger(""))
                .As(typeof(ILog));


            containerBuilder.RegisterModule<LogInjectionModule>();
            containerBuilder.RegisterType<LogProperties>().As<ILogProperties>().SingleInstance();
            containerBuilder.RegisterType<PublicInterfaceLoggingInterceptor>().SingleInstance();
        }
    }
}
