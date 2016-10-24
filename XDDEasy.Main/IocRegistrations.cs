using System.Linq;
using Autofac;
using Autofac.Integration.Mvc;
using Common.Ioc;

namespace XDDEasy.Main
{
    public class IocRegistrations : IRegister
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            var assemblies = AssemblyHelper.Assemblies;
            containerBuilder.RegisterControllers(assemblies.ToArray());
        }
    }
}