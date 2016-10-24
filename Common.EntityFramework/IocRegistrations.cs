using Autofac;
using Common.EntityFramework.Config;
using Common.EntityFramework.DataAccess;
using Common.Ioc;

namespace Common.EntityFramework
{
    public class IocRegistrations : IRegister
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterGeneric(typeof(GenericRepository<>))
               .As(typeof(IRepository<>))
               .InstancePerLifetimeScope()
               .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            containerBuilder.RegisterType<UnitOfWork>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            containerBuilder.RegisterType<LogCommandInterceptor>().SingleInstance();

            containerBuilder.RegisterType<EfDbConfiguration>().SingleInstance();
        }
    }
}
