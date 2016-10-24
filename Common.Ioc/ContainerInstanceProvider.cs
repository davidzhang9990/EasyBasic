using System;
using System.Diagnostics;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace Common.Ioc
{
    public static class ContainerInstanceProvider
    {
        public static IContainer GetContainerInstance()
        {
            return GetContainerInstance(Assembly.GetCallingAssembly());
        }

        public static IContainer GetContainerInstance(Assembly callingAssembly)
        {
            var containerBuilder = new ContainerBuilder();
            foreach (Type type in TypeHelper.FindTypesThatExtend<IRegister>(AssemblyHelper.AssembliesOrdered("Common", callingAssembly.FullName)))
            {
                Trace.WriteLine("Executing IRegister.Register method within type " + type);
                ((IRegister)Activator.CreateInstance(type)).Register(containerBuilder);
            }
            IContainer container2 = containerBuilder.Build(ContainerBuildOptions.None);
            //Contract.Ensures(container != null, "Contract.Result<IContainer>() != null");
            return container2;
        }
    }
}
